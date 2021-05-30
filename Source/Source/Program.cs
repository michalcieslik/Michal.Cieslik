using System;
using System.IO;

namespace swi
{
    class Program
    {
        //convertion from secends to hours:minutes:secends
        public static string ConvertToString(int secends)
        {
            int minutes = secends / 60;
            int hours = minutes / 60;
            string final = Convert.ToString(hours + ":" + (minutes - (hours * 60)) + ":" + (secends - (minutes * 60)));
            return final;
        }

        //check if it is weekend
        public static bool IsItWeekend(string date)
        {
            string[] splitDate = date.Split("-");
            var date1 = new DateTime(Convert.ToInt32(splitDate[0]), Convert.ToInt32(splitDate[1]), Convert.ToInt32(splitDate[2]));
            if (date1.DayOfWeek == DayOfWeek.Saturday || date1.DayOfWeek == DayOfWeek.Sunday)
            {
                return true;
            }
            return false;
        }

        static void Main(string[] args)
        {
            //getting path to file
            string path = Environment.CurrentDirectory;
            path = path + @"\input.csv";
            //open file
            string[] lines = System.IO.File.ReadAllLines(path);

            //some global 
            int amountOfDays = 0;
            int timeSpentWorking = 0;

            //initialization of main array
            string[][] data = new string[lines.Length - 1][];
            for (int i = 0; i < lines.Length - 1; i++)
            {
                data[i] = new string[4];
            }

            //make array with grouped data first is day second time third entry or exit and last is gate number
            for (int i = 1; i < lines.Length; i++)
            {
                string[] splitLine = lines[i].Split(";");
                string[] dayAndTime = splitLine[0].Split();
                string[] time = dayAndTime[1].Split(":");
                //instead of time in structure we have which secend of the day it is
                string timeInSecends = Convert.ToString((Convert.ToInt32(time[0]) * 3600) + (Convert.ToInt32(time[1]) * 60) + Convert.ToInt32(time[2]));
                string[] entryOrExit = splitLine[1].Split(" ");
                string[] whichDoor = splitLine[2].Split("/");
                data[i - 1][0] = dayAndTime[0];
                data[i - 1][1] = timeInSecends;
                data[i - 1][2] = entryOrExit[1];
                data[i - 1][3] = whichDoor[1];
            }

            string currentDay = data[0][0];
            int start = Convert.ToInt32(data[0][1]);
            int stop = 0;
            int dayTime = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (i == data.Length - 1)
                {
                    //last iteration
                    stop = Convert.ToInt32(data[i][1]);
                    dayTime = dayTime + (stop - start);
                    string additional = "";
                    if (data[i][2].Equals("entry"))
                    {
                        additional = additional + " i";
                    }
                    if (dayTime > 9 * 3600)
                    {
                        additional = additional + " ot";
                    }
                    if (dayTime < 6 * 3600)
                    {
                        additional = additional + " ut";
                    }
                    if (IsItWeekend(currentDay))
                    {
                        additional = additional + " w";
                    }

                    timeSpentWorking = timeSpentWorking + dayTime;
                    amountOfDays++;
                    string time = ConvertToString(dayTime);
                    string allTime = ConvertToString(timeSpentWorking);
                    string moreOrLess = ConvertToString(timeSpentWorking - (amountOfDays * 8 * 3600));
                    Console.WriteLine($"Day {currentDay} Work {time} {additional} {allTime} {moreOrLess}");
                    break;
                }
                else if (data[i][0].Equals(currentDay))
                {
                    if (start == 0 && data[i][2].Equals("entry") && data[i][3].Equals("0"))
                    {
                        start = Convert.ToInt32(data[i][1]);
                    }
                    else if (start != 0 && data[i][2].Equals("exit") && data[i][3].Equals("0"))
                    {
                        stop = Convert.ToInt32(data[i][1]);
                        dayTime = dayTime + (stop - start);
                        stop = 0;
                        start = 0;
                    }
                }
                else
                {
                    //print how much worked in the day

                    //check if is needed to add some specifics at the end
                    string additional = "";
                    if (data[i - 1][2].Equals("entry"))
                    {
                        dayTime = dayTime + (Convert.ToInt32(data[i - 1][1]) - start);
                        additional = additional + " i";
                    }
                    if (dayTime > 9 * 3600)
                    {
                        additional = additional + " ot";
                    }
                    if (dayTime < 6 * 3600)
                    {
                        additional = additional + " ut";
                    }
                    if (IsItWeekend(currentDay))
                    {
                        additional = additional + " w";
                    }
                    string time = ConvertToString(dayTime);
                    Console.WriteLine($"Day {currentDay} Work {time} {additional}");
                    currentDay = data[i][0];
                    i--;
                    start = 0;
                    timeSpentWorking = timeSpentWorking + dayTime;
                    dayTime = 0;
                    amountOfDays++;
                }
            }
            Console.WriteLine("Click enter to end program");
            Console.Read();
        }
    }
}
