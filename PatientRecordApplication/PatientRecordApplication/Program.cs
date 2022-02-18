using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace PatientRecordApplication
{
    
    class Program
    {
        static void Main(string[] args)
        {
            EnterPatients();
            DisplayPatients();
            FindIDNumber();
            FindPatientBalance();
        }

        //Prompts user to enter patients into file
        static void EnterPatients()
        {
            /*char s;
            s = Convert.ToChar(Console.ReadLine());
            try
            {
                if(s == 'L')
                {
                    throw new RatioException("L + Ratio");
                }
            }
            catch(RatioException e)
            {
                Console.Write("{0}, Get Ratioed\n",e.Message);
            }*/
            const int END = 999;
            const string DELIM = ",";
            const string FILENAME = "PatientData.txt";
            Patient pat = new Patient();
            FileStream outFile = new FileStream(FILENAME,
                FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(outFile);
            Console.Write("Enter patient ID number or " + END +
                " to quit >> ");
            pat.PatNum = Convert.ToInt32(Console.ReadLine());
            while (pat.PatNum != END)
            {
                Console.Write("Enter last name >> ");
                pat.Name = Console.ReadLine();
                Console.Write("Enter balance >> ");
                try //Looks for format exception error
                {
                    pat.Balance = Convert.ToDouble(Console.ReadLine());
                }
                catch(System.FormatException) //Handles format exception error
                {
                    Beginning:
                    Console.Write("Error, Enter new balance >> ");
                    string temp = Console.ReadLine();
                    foreach (char t in temp)
                    {
                        if (!(t >= 48 && t <= 57))
                        {
                            goto Beginning;
                        }
                    }
                    pat.Balance = Convert.ToDouble(temp);
                }
                writer.WriteLine(pat.PatNum + DELIM + pat.Name +
                    DELIM + pat.Balance);
                Console.Write("Enter next patient ID number or " +
                    END + " to quit >> ");
                pat.PatNum = Convert.ToInt32(Console.ReadLine());
            }
            writer.Close();
            outFile.Close();
        }
        //Displays the patients in the file
        static void DisplayPatients()
        {
            const char DELIM = ',';
            const string FILENAME = "PatientData.txt";
            Patient pat = new Patient();
            FileStream inFile = new FileStream(FILENAME,
                FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(inFile);
            string recordIn;
            string[] fields;
            Console.WriteLine("\n{0,-5}{1,-12}{2,8}\n",
                "Num", "Name", "Salary");
            recordIn = reader.ReadLine();
            while (recordIn != null)
            {
                fields = recordIn.Split(DELIM);
                pat.PatNum = Convert.ToInt32(fields[0]);
                pat.Name = fields[1];
                pat.Balance = Convert.ToDouble(fields[2]);
                Console.WriteLine("{0,-5}{1,-12}{2,8}",
                    pat.PatNum, pat.Name, pat.Balance.ToString("C"));
                recordIn = reader.ReadLine();
            }
            reader.Close();
            inFile.Close();
        }
        //Finds the ID number if it exists
        static void FindIDNumber()
        {
            const char DELIM = ',';
            const int END = 999;
            const string FILENAME = "PatientData.txt";
            Patient pat = new Patient();
            FileStream inFile = new FileStream(FILENAME,
                FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(inFile);
            string recordIn;
            string[] fields;
            int id;
            bool found;
            Console.Write("Enter ID to find or " +
                END + " to quit >> ");
            id = Convert.ToInt32(Console.ReadLine());
            while (id != END)
            {
                found = false;
                Console.WriteLine("\n{0,-5}{1,-12}{2,8}\n",
                    "Num", "Name", "Salary");
                inFile.Seek(0, SeekOrigin.Begin);
                recordIn = reader.ReadLine();
                while (recordIn != null)
                {
                    fields = recordIn.Split(DELIM);
                    pat.PatNum = Convert.ToInt32(fields[0]);
                    pat.Name = fields[1];
                    pat.Balance = Convert.ToDouble(fields[2]);
                    if (pat.PatNum == id)
                    {
                        Console.WriteLine("{0,-5}{1,-12}{2,8}", pat.PatNum,
                            pat.Name, pat.Balance.ToString("C"));
                        found = true;
                    }
                    recordIn = reader.ReadLine();
                }
                try //Looks for if the id doesnt exist
                {
                    if (!found)
                    {
                        throw new IDNotFoundException("ID not found in file");
                    }
                }
                catch(IDNotFoundException ex) // catches if ID entered wasn't found
                {
                    Console.Write("{0} {1}", ex.Message, FILENAME);
                }
                Console.Write("\nEnter ID to find or " +
                    END + " to quit >> ");
                id = Convert.ToInt32(Console.ReadLine());
            }
            reader.Close();  // Error occurs if
            inFile.Close(); //these two statements are reversed
        }
        //Finds all patient balances that are greater than minimum given
        static void FindPatientBalance()
        {
            const char DELIM = ',';
            const int END = 999;
            const string FILENAME = "PatientData.txt";
            Patient pat = new Patient();
            FileStream inFile = new FileStream(FILENAME,
                FileMode.Open, FileAccess.Read);
            StreamReader reader = new StreamReader(inFile);
            string recordIn;
            string[] fields;
            double minSalary;
            Console.Write("Enter minimum salary to find or " +
                END + " to quit >> ");
            minSalary = Convert.ToDouble(Console.ReadLine());
            while (minSalary != END)
            {
                Console.WriteLine("\n{0,-5}{1,-12}{2,8}\n",
                    "Num", "Name", "Salary");
                inFile.Seek(0, SeekOrigin.Begin);
                recordIn = reader.ReadLine();
                while (recordIn != null)
                {
                    fields = recordIn.Split(DELIM);
                    pat.PatNum = Convert.ToInt32(fields[0]);
                    pat.Name = fields[1];
                    pat.Balance = Convert.ToDouble(fields[2]);
                    if (pat.Balance >= minSalary)
                        Console.WriteLine("{0,-5}{1,-12}{2,8}", pat.PatNum,
                            pat.Name, pat.Balance.ToString("C"));
                    recordIn = reader.ReadLine();
                }
                Console.Write("\nEnter minimum salary to find or " +
                    END + " to quit >> ");
                minSalary = Convert.ToDouble(Console.ReadLine());
            }
            reader.Close();  // Error occurs if
            inFile.Close(); //these two statements are reversed
        }
    }
    class Employee
    {
        public int EmpNum { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
    }

    class Person
    {
        public int EmpNum { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
    }
    
    class Patient
    {
        public int PatNum { get; set; }
        public string Name { get; set; }
        public double Balance { get; set; }
    }

    class IDNotFoundException: Exception
    {
        public IDNotFoundException(string message): base(message)
        {
        }
    }

    /*class RatioException: Exception
    {
        public RatioException(string message): base(message)
        {
        }
    }*/
}
