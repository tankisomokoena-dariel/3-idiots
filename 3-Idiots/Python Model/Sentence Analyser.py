from flask import Flask
from flask_restful import Resource, Api, reqparse
import nltk
import pandas as pd
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
server = 'dariel3idiots.database.windows.net'
db = 'darielihub'
uname = 'dariel3diots'
password = 'dariel3idiots!'
driver = '{ODBC Driver 17 for SQL Server}'
qna = dict()
questions = []
users = []
map_users = dict()
map_xpts = {}


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
                cursor.execute("SELECT qaID, question, answer FROM dbo.QandA")
                row = cursor.fetchall()

                for i in range(len(row)):
                    questions.append(row[i])


                for i in range(len(questions)):
                    qna[i] = list()
                    qna[i].append(questions[i][-2])
                    qna[i].append(questions[i][-1])


        # GET USERS
        with pyodbc.connect('DRIVER='+driver+';SERVER=tcp:'+server+
                            ';PORT=1433;DATABASE='+db+';UID='+uname+';PWD='+password) as conn:
            with conn.cursor() as cursor:
                cursor.execute('select * from [dbo].[SkillDescription] JOIN [dbo].[Skills] on '+ 
                               '[dbo].[Skills].skillID = [dbo].[SkillDescription].skillID ' +
                               'JOIN '+
                               '[dbo].[userTable] '+
                               'on '+
                               '[dbo].[Skills].userID = [dbo].[userTable].userID')
                row = cursor.fetchall()

                for i in range(len(row)):
                    users.append(row[i])

        print("DATABASE RUN COMPLETED")


        for i in range(len(users)):
            map_users[users[i][0]] = list()
            map_users[users[i][0]].append(users[i][8])
            map_users[users[i][0]].append(users[i][9])
            map_users[users[i][0]].append(users[i][1])
            
# =============================================================================
#         print(map_users)
#         print(qna)
# =============================================================================


    def csv_run(self):
        df = pd.read_csv('Compressed_Skills.csv')
         
            
        map_users_csv = df[['Expert', 'Recognized Skil Reference:Title']]

        c = map_users_csv.shape[0]

        

        for i in range(c):
            
            dirty_xpt_list = map_users_csv['Expert'].iloc[i]
            dirty_skill_list = map_users_csv['Recognized Skil Reference:Title'].iloc[i]
            
            dirty_xpt_list = str(dirty_xpt_list)
            dirty_skill_list = str(dirty_skill_list)
            
            if dirty_xpt_list == "nan":
                dirty_xpt_list = "no expert found"
                print(dirty_xpt_list)
            else:
                
                xpt_array = dirty_xpt_list.split(";")
                skill_array = dirty_skill_list.split(";")
                
                xpt_list_former = ''
                skill_list_former = ''
                
                for j in range(len(xpt_array)):
                    
                    if j < len(xpt_array)-1:
                        xpt_list_former = xpt_list_former + xpt_array[j] + ',' 
                    else:
                        xpt_list_former = xpt_list_former + xpt_array[j]
                        
                
                for k in range(len(skill_array)):
                    if k < len(skill_array)-1:
                        skill_list_former = skill_list_former + skill_array[k] + ',' 
                    else:
                        skill_list_former = skill_list_former + skill_array[k]
                   
                
                map_xpts[i] = [xpt_list_former, skill_list_former]
                
        print("CSV FILE RUN COMPLETED")
        
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
        #self.csv_run()
        
        
        similar_q = {}
        expert = {}

        start = self.get_time()

        for i in qna:

# =============================================================================
#             data fetched from the data based is explicitly converted
#             into a string using str() to ensure no special or illegal
#             characters cause internal server errors
# =============================================================================

            ideal_q = str(qna[i][0])
            
# =============================================================================
#             print("\n")
#             print("i : "+ str(i))
#             print("QUESTION: "+ search_q)
#             print("DB QUESTION: "+ ideal_q)
# =============================================================================

            ans = qna[i][1]

            sen_ratio = self.match_sen(search_q,ideal_q)
            
# =============================================================================
#             print("RATIO: "+ str(sen_ratio))
# =============================================================================

            if (sen_ratio> 0.3):
                sen_ratio = sen_ratio*100
                similar_q[sen_ratio] = list()
                # similar_q[sen_ratio].append(ideal_q)
                similar_q[sen_ratio].append(ans)



# =============================================================================
#         print(map_users)
# =============================================================================
        
        shift_ratio = 0
        for j in map_users:

            xpt_obj = map_users[j]
           
            
            xpt = xpt_obj[0]+' '+xpt_obj[1]
            
# =============================================================================
#             print("EXPERT: "+ xpt)
# =============================================================================
            
            skill_list = xpt_obj[-1]
            
            
            xpt_ratio = self.match_sen(search_q,skill_list)


            if (xpt_ratio> 0.3 and xpt_ratio != shift_ratio):
                xpt_ratio = xpt_ratio*100
                expert[xpt_ratio] = list()
                expert[xpt_ratio].append(xpt)
                xpt_ratio = xpt_ratio/100
                #expert[xpt_ratio].append(skill_list)


            if (xpt_ratio> 0.3 and xpt_ratio == shift_ratio):
                xpt_ratio = xpt_ratio*100
                xpt_ratio = xpt_ratio - 0.2123
                expert[xpt_ratio] = list()
                expert[xpt_ratio].append(xpt)

            shift_ratio = xpt_ratio

        end = self.get_time()
        print((end - start)/1000, 'seconds')


        sort_q = collections.OrderedDict(sorted(similar_q.items(), reverse=True))


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


        print(sort_q)
        if len(sort_q) == 0 and len(sort_xpt) == 0:
            return {"output": "no search results."}
        else:

            if len(sort_q) == 0:
                return {'answer' : "no answer found for question",
                        'expert' : experts}

            elif len(sort_xpt) == 0:
                 return    {'answer' : answers,
                         'expert' : "no experts found"}

            else:
                return {'answer' : answers,
                    'expert' : experts}
    pass


api.add_resource(Questions, '/question')

if __name__ == '__main__':
    app.run(host="0.0.0.0", port=5000, debug=False)
