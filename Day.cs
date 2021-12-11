using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Diagnostics;
namespace accountant
{
   public class Day
    {
       public string Days { get; set; }
        public int TotalGains { get; set; }
        public int TotalSells { get; set; }
        public static Collection<ObservableCollection<Day>> GetWeekDetails(int month, int year)
        {
            ObservableCollection<Day> firstWeek = new ObservableCollection<Day>();
            ObservableCollection<Day> secondWeek = new ObservableCollection<Day>();
            ObservableCollection<Day> thirdWeek = new ObservableCollection<Day>();
            ObservableCollection<Day> fourthWeek = new ObservableCollection<Day>();
            ObservableCollection<Sellings> sellingsList = new ObservableCollection<Sellings>();
            int totalGain = 0;
            int totalSellings = 0;
            sellingsList = Sellings.RetrieveByMonth(month, 2021);
            bool  firstWeekFilled = false;
            bool secondWeekFilled = false;
            bool thirdWeekFilled = false;
            if (sellingsList.Count > 0)
            {
                
                for (int i = 0; i < sellingsList.Count ; i++)
                {
                    
                    if (i == 0 || sellingsList[i].Inserted.Day== sellingsList[i - 1].Inserted.Day && i != sellingsList.Count - 1)
                    {
                        totalGain += sellingsList[i].Gain;
                        totalSellings += sellingsList[i].SellingPrice * sellingsList[i].Number;
                        
                    }
                  
                    else if (i != 0  && sellingsList[i].Inserted.Day != sellingsList[i - 1].Inserted.Day || i== sellingsList.Count - 1)
                    {
                        
                        if (i== sellingsList.Count -1 && sellingsList[i].Inserted.Day == sellingsList[i-1].Inserted.Day)
                        {
                            totalGain += sellingsList[i].Gain;
                            totalSellings += sellingsList[i].SellingPrice * sellingsList[i].Number;
                        }
                       else if (i == sellingsList.Count - 1 && sellingsList[i].Inserted.Day != sellingsList[i - 1].Inserted.Day)
                        {
                            if (!firstWeekFilled)
                            {

                                firstWeek.Add(new Day()
                                {
                                    Days = sellingsList[i].Inserted.Date.ToString()
                                                           ,
                                    TotalGains = sellingsList[i].Gain
                                                            ,
                                    TotalSells = sellingsList[i].Number * sellingsList[i].SellingPrice
                                });
                            }
                            else if (!secondWeekFilled)
                            {
                                secondWeek.Add(new Day()
                                {
                                    Days = sellingsList[i].Inserted.Date.ToString()
                                                          ,
                                    TotalGains = sellingsList[i].Gain
                                                           ,
                                    TotalSells = sellingsList[i].Number * sellingsList[i].SellingPrice
                                });
                            }
                            else if (!thirdWeekFilled)
                            {
                                thirdWeek.Add(new Day()
                                {
                                    Days = sellingsList[i].Inserted.Date.ToString()
                                                          ,
                                    TotalGains = sellingsList[i].Gain
                                                           ,
                                    TotalSells = sellingsList[i].Number * sellingsList[i].SellingPrice
                                });
                            }
                            else
                            {
                                fourthWeek.Add(new Day()
                                {
                                    Days = sellingsList[i].Inserted.Date.ToString()
                                                             ,
                                    TotalGains = sellingsList[i].Gain
                                                              ,
                                    TotalSells = sellingsList[i].Number * sellingsList[i].SellingPrice
                                });
                            }
                        }
                        if (firstWeek.Count < 8)
                        {
                            firstWeek.Add(new Day()
                            {
                                Days = sellingsList[i - 1].Inserted.Date.ToString()
                                                       ,
                                TotalGains = totalGain
                                                        ,
                                TotalSells = totalSellings
                            });
                            totalGain = sellingsList[i].Gain;
                            totalSellings = sellingsList[i].SellingPrice * sellingsList[i].Number;
                            

                        }
                        else
                        {
                            firstWeekFilled = true;
                            if (secondWeek.Count < 8)
                            {
                                secondWeek.Add(new Day()
                                {
                                    Days = sellingsList[i - 1].Inserted.Date.ToString()
                                                           ,
                                    TotalGains = totalGain
                                                            ,
                                    TotalSells = totalSellings
                                });
                                totalGain = sellingsList[i].Gain;
                                totalSellings = sellingsList[i].SellingPrice * sellingsList[i].Number;

                            }
                            else
                            {
                                secondWeekFilled = true;
                                if (thirdWeek.Count < 8)
                                {
                                    thirdWeek.Add(new Day()
                                    {
                                        Days = sellingsList[i - 1].Inserted.Date.ToString()
                                                               ,
                                        TotalGains = totalGain
                                                                ,
                                        TotalSells = totalSellings
                                    });
                                    totalGain = sellingsList[i].Gain;
                                    totalSellings = sellingsList[i].SellingPrice * sellingsList[i].Number;

                                }
                                else
                                {
                                    thirdWeekFilled = true;
                                    fourthWeek.Add(new Day()
                                    {
                                        Days = sellingsList[i - 1].Inserted.Date.ToString()
                                                                    ,
                                        TotalGains = totalGain
                                                                     ,
                                        TotalSells = totalSellings
                                    });
                                    totalGain = sellingsList[i].Gain;
                                    totalSellings = sellingsList[i].SellingPrice * sellingsList[i].Number;
                                }
                            }
                        }
                    }
                }

            }
            Collection<ObservableCollection<Day>> result = new Collection<ObservableCollection<Day>>();
            result.Add(firstWeek);
            result.Add(secondWeek);
            result.Add(thirdWeek);
            result.Add(fourthWeek);
            return result;
        }
    }
   
}
