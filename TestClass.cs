using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.ObjectModel;
namespace accountant
{
  public  class TestClass
    {
        public static  async void DummyDataProduct()
        {
           
            for (int i =0;i<1001; i++)
            {
               bool done=  Product.InsertProduct(new Product()
                {
                    ProductId=i.ToString(),
                    ProductName=$"product{i}",
                    Price=5000,
                    ProductNumber=5,
                    SellingPrice=7000,
                    
                });
                if (!done)
                {
                    Trace.WriteLine($"something wrong happen in Dummy products {i}");
                    break;
                }
            }
        }

        public static void DummyDataSellings()
        {
            List<Sellings> lst = new List<Sellings>();
            ObservableCollection<Product> products = new ObservableCollection<Product>();
            products = Product.showStore();
           

            for (int y = 0; y < 1; y++)
            {
                for (int m = 1; m < 13; m++)
                {
                    int monthLenght = 31;
                    if (m == 2) monthLenght = 29;
                    for (int d = 1; d < monthLenght; d++)
                    {
                        for (int i = 0; i < 200; i++)
                        {
                            string dateString = $"{m}/{d}/{2021 + y}";
                            DateTime date = DateTime.Parse(dateString);
                            lst.Add(new Sellings()
                            {
                                ProductId = i.ToString(),
                                ProductName = $"product{i}",
                                SellingPrice = 2000,
                                Gain=1000,
                                Inserted=date,
                                Number=1,

                            }) ;
                        }
                       
                    }
                   


            }
            }
            List<Product> listOfProducts = new List<Product>();
            listOfProducts.Concat(products);
            Sellings.InsertProduct(lst, listOfProducts);

        }
        public static async void DummyDataBoxOperation()
        {

            foreach (string name in Box.GetAllBoxNames())
            {
                for (int i = 0; i < 2; i++)
                {
                    for (int j = 1; j < 13; j++)
                    {
                        for (int m = 1; m < 29; m++)
                        {
                            string type = "revenue";
                            string expenseSource = "";
                            if (i % 2 == 0) { type = "expense"; expenseSource = "cost"; }
                            bool done =  Operation.SetOperation (new Operation()
                            {
                                Name = name,
                                Type = type,
                                CostPrice = 20000 +i,
                                ExpenseSource = expenseSource,
                                MoneyAmmount = 70000 +j,
                                Inserted = DateTime.Parse($"{j}/{m}/2021"),
                                

                            });
                            if (!done)
                            {
                                Trace.WriteLine("something wrong happen in Dummy Sellings");
                                break;
                            }
                        }
                    }

                }
            }
        }
    }
}
