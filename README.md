# FarmersMarket

AS1 Application Development
Farmers’ market in Montreal have decided to manage their business using software application. You are asked to make a small desktop application so that they can keep track of all of their sales perfectly. 

Farmers’ market coordinator has shared you the following dataset: 
Product Name Product ID Amount(kg) Price(CAD)/kg 
Apple 124567 23 2.10 
Orange 345678 20 2.49 
Raspberry 125678 25 2.35 
Blueberry 456732 29 1.45 
Cauliflower 238901 24 2.22  

As a project dealer, you have to create a desktop application which will contain the following functionalities and will help the coordinator to track the sales and inventory together: 

1. You have to make one WPF class, name Admin, to SELECT, INSERT, UPDATE and DELETE any of the products and their amount listed in the table. This class should use GridView to show all the records in the database and will help the coordinator to know the inventory as well as update when requires.  

2. You have to make another WPF class, name Sales, to calculate the total sales for a customer X. Customer will take any product with any amount as they wish from the given products in Database. Based on the customer’s chosen product, the total sales amount will be updated as well as the inventory. E.g. Customer X has chosen Apple 2kg and Raspberry 3kg. The total sales amount will be (2 * 2.10 + 3 * 2.35) which will be displayed in the WPF screen. At the same time, the inventory will be deduct and update the amount customer has taken, such as Apple amount will be (23-2)~ 21 kg and Raspberry amount will be (25-3)~22 kg which will be updated in the database. This process will continue for all the customers. Once a customer perform their buy operation, if you move to Admin WPF class, you should see the updated inventory information.  

3. You have to create and push all of your works in your team GitHub repository. I will check how many Commits each member has done as well as which part you work.  

4. Bonus: You have to use individual thread for each method, instruction or SQL operation. This will ensure 10% additional points in this assignment
