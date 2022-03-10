How to run the system

1. Run the API
2. On the swagger UI that appears click the link '/swagger/v1/swagger.json' below "My Title"
3. Then copy the URL of the JSON page that you get. The URL should something similar to "https://localhost:5001/swagger/v1/swagger.json"



4. Now open your NSWAG studio application
5. Paste the URL you copied in step 3 in the field of "Specification URL" and click the "create local copy" button



6. Then look for the heading that says "Outputs" and below it check the field that says "CSharp Client"
7. Then click on the tab that says "CSharp Client" next to "OpenAPI/Swagger Specification"



8. Now, under settings rename the namespace to IdiotsAPI
9. Then scroll down and look for the heading that says "Excluded Parameter Names (comma separated)", and below it uncheck the textbox next to "Inject HttpClient via constructor"
10. Then scroll to the bottom and look for the "output file path" and write the path where you want to save the file to be generated, e.g C:\Hackathon Project\3-idiots\3-Idiots\API\Client.cs
For simplicity purposes, you can make the path refer to the API folder that is in our frontend project as I did in the example path I mentioned above
11. Now you can click the "Generate Files" button, next to the "Generate Output" and your CSharp file will be created and saved on the specified path

Python Model

The python model utilised is a sentence processer that analyses strings to find strings that match. It matches searched questions with frequently asked questions that has been asked already. The model can also find user skills related to the searched question.

