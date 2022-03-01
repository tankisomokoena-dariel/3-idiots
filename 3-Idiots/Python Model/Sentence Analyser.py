from flask import Flask
from flask_restful import Resource, Api, reqparse
import csv
import nltk
import nltk.corpus
nltk.download('stopwords')
import nltk.tokenize.punkt
import nltk.stem.snowball
import time
import collections
from nltk.tokenize import WordPunctTokenizer
import string

import pyodbc

# Server Variables
server = '3idiots.database.windows.net'
db = '3idiots'
uname = 'idiots' 
password = 'Dariel@123'
driver = '{ODBC Driver 17 for SQL Server}'
qna = dict()
questions = []
users = []
map_users = dict()


# Get default English stopwords and extend with punctuation
stopwords = nltk.corpus.stopwords.words('english')
stopwords.extend(string.punctuation)
stopwords.append('')

# Create tokenizer and stemmer
tokenizer = WordPunctTokenizer()

app = Flask(__name__)
api = Api(app)

# Import CSV for member skills


# HTTP requestion class
class Questions(Resource):
    
    
    def database_run(self):
        # GET QUESTIONS
        with pyodbc.connect('DRIVER='+driver+';SERVER=tcp:'+server+
                            ';PORT=1433;DATABASE='+db+';UID='+uname+';PWD='+password) as conn:
            with conn.cursor() as cursor:
                cursor.execute("SELECT qaID, question, answer FROM QandA")
                row = cursor.fetchall()
                
                for i in range(len(row)):
                    questions.append(row[i])
                    
                
                for i in range(len(questions)):
                    qna[questions[i][0]] = list()
                    qna[questions[i][0]].append(questions[i][-2])
                    qna[questions[i][0]].append(questions[i][-1])
                    
                    
        # GET USERS
        with pyodbc.connect('DRIVER='+driver+';SERVER=tcp:'+server+
                            ';PORT=1433;DATABASE='+db+';UID='+uname+';PWD='+password) as conn:
            with conn.cursor() as cursor:
                cursor.execute("SELECT userID, firstName, lastName, skill FROM userTable WHERE skill IS NOT NULL")
                row = cursor.fetchall()
                
                for i in range(len(row)):
                    users.append(row[i])
                
        print("Database Run Completed Successfully ;)")
        
        
        for i in range(len(users)):
            map_users[users[i][0]] = list()
            map_users[users[i][0]].append(users[i][-3])
            map_users[users[i][0]].append(users[i][-2])
            map_users[users[i][0]].append(users[i][-1])        
        
    
    # CALCULATE TIME
    def get_time(self):
      return round(time.time() * 1000)
  

    
    # SENTENCE SIMILARITY / FIND EXPERT
    def match_sen(self, a, b, threshold=0.5):
    
        tokens_a = [token.lower().strip(string.punctuation) for token in tokenizer.tokenize(a) \
                        if token.lower().strip(string.punctuation) not in stopwords]
        tokens_b = [token.lower().strip(string.punctuation) for token in tokenizer.tokenize(b) \
                        if token.lower().strip(string.punctuation) not in stopwords]
    
        # Calculate Jaccard similarity
        ratio = len(set(tokens_a).intersection(tokens_b)) / float(len(set(tokens_a).union(tokens_b)))
        # print(ratio)    
        return (ratio)
   

    def get(self):
        parser = reqparse.RequestParser();
        
        #parser.add_argument('qs', required=True)
        parser.add_argument('q', required=True)
        
        args = parser.parse_args()
        
        #questions = args['qs'].split("%")
        search_q = args['q']
        
        
        self.database_run()
        similar_q = {}
        expert = {}
        
        start = self.get_time()
             
        for i in qna:
            ideal_q = qna[i][-2]
            ans = qna[i][-1]
            
            sen_ratio = self.match_sen(search_q,ideal_q)*100
            
            
            if (sen_ratio> 0.3):
              similar_q[sen_ratio] = list()
              # similar_q[sen_ratio].append(ideal_q)
              similar_q[sen_ratio].append(ans)
              
              
            
        shift_ratio = 0     
        for j in map_users:
            
            xpt_obj = map_users[j]
            xpt = xpt_obj[-3]+' '+xpt_obj[-2]
            skill_list = xpt_obj[-1]
            xpt_ratio = self.match_sen(search_q,skill_list)*100
            
            
            if (xpt_ratio> 0.3 and xpt_ratio != shift_ratio):
                expert[xpt_ratio] = list()
                expert[xpt_ratio].append(xpt)
                #expert[xpt_ratio].append(skill_list)
            
            if (xpt_ratio> 0.3 and xpt_ratio == shift_ratio):
                xpt_ratio = xpt_ratio - 0.2123
                expert[xpt_ratio] = list()
                expert[xpt_ratio].append(xpt)
                #expert[xpt_ratio].append(skill_list)
    
            shift_ratio = xpt_ratio
              
        end = self.get_time()
        print((end - start)/1000, 'seconds')


        sort_q = collections.OrderedDict(sorted(similar_q.items(), reverse=True))
        
        
        print(sort_q)
        
        
        answers = ''
        counter = 0
        for ans in sort_q:
            
            ans_list = sort_q[ans]
            
            if counter < len(sort_q)-1:
                answers = answers + ans_list[0] + ','
            else:
                answers = answers + ans_list[0]
                
            counter = counter + 1


        
        sort_xpt = collections.OrderedDict(sorted(expert.items(), reverse=True))
        
        experts = ''
        counter = 0
        for xpt in sort_xpt:
            xpt_list = sort_xpt[xpt]
               
            if counter < len(sort_xpt)-1:
                experts = experts + xpt_list[0] + ','
            else:
                experts = experts + xpt_list[0] 
                
            counter = counter + 1
        
     
        
        if len(sort_q) == 0 and len(sort_xpt) == 0:
        
            return {"output": "no search results."}
        else:
            return {'answer' : answers,
                    'expert' : experts}
    pass


api.add_resource(Questions, '/question')

if __name__ == '__main__':
    app.run(host="0.0.0.0", port=9000, debug=False)