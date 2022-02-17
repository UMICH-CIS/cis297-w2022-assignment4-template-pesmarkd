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
            //FileOperations();
            //DirectoryOperations();
            FileStreamOperations();
            SequentialAccessWriteOperation();
            ReadSequentialAccessOperation();
            FindEmployees();
            SerializableDemonstration();
        }
        //File operations
        static void FileOperations()
        {
            string fileName;
            Console.Write("Enter a filename >> ");
            fileName = Console.ReadLine();
            if (File.Exists(fileName))
            {
                Console.WriteLine("File exists");
                Console.WriteLine("File was created " +
                    File.GetCreationTime(fileName));
                Console.WriteLine("File was last written to " +
                    File.GetLastWriteTime(fileName));
            }
            else
            {
                Console.WriteLine("File does not exist");
            }
        }
        //Directory Operations
        static void DirectoryOperations()
        {
            //Directory operations
            string directoryName;
            string[] listOfFiles;
            Console.Write("Enter a folder >> ");
            directoryName = Console.ReadLine();
            if (Directory.Exists(directoryName))
            {
                Console.WriteLine("Directory exists, and it contains the following:");
                listOfFiles = Directory.GetFiles(directoryName);
                for (int x = 0; x < listOfFiles.Length; ++x)
                    Console.WriteLine("   {0}", listOfFiles[x]);
            }
            else
            {
                Console.WriteLine("Directory does not exist");
            }
        }
        //Using FileStream to create and write some text into it
        static void FileStreamOperations()
        {
            FileStream outFile = new
            FileStream("SomeText.txt", FileMode.Create,
            FileAccess.Write);
            StreamWriter writer = new StreamWriter(outFile);
            Console.Write("Enter some text >> ");
            string text = Console.ReadLine();
            writer.WriteLine(text);
            // Error occurs if the next two statements are reversed
            writer.Close();
            outFile.Close();
        }
        //Writing data to a Sequential Access text file
        static void SequentialAccessWriteOperation()
        {
            const int END = 999;
            const string DELIM = ",";
            const string FILENAME = "EmployeeData.txt";
            Employee emp = new Employee();
            FileStream outFile = new FileStream(FILENAME,
                FileMode.Create, FileAccess.Write);
            StreamWriter writer = new StreamWriter(outFile);
            Console.Write("Enter employee number or " + END +
                " to quit >> ");
            emp.EmpNum = Convert.ToInt32(Console.ReadLine());
            while (emp.EmpNum != END)
            {
                Console.Write("Enter last name >> ");
                emp.Name = Console.ReadLine();
                Console.Write("Enter salary >> ");
                emp.Salary = Convert.ToDouble(Console.ReadLine());
                writer.WriteLine(emp.EmpNum + DELIM + emp.Name +
                    DELIM + emp.Salary);
                Console.Write("Enter next employee number or " +
                    END + " to quit >> ");
                emp.EmpNum = Convert.ToInt32(Console.ReadLine());
            }
            writer.Close();
            outFile.Close();
        }
        //Read data from a Sequential Access File
        static void ReadSequentialAccessOperation()
        {
            const char DELIM = ',';
            const string FILENAME = "EmployeeData.txt";
            Employee emp = new Employee();
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
                emp.EmpNum = Convert.ToInt32(fields[0]);
                emp.Name = fields[1];
                emp.Salary = Convert.ToDouble(fields[2]);
                Console.WriteLine("{0,-5}{1,-12}{2,8}",
                    emp.EmpNum, emp.Name, emp.Salary.ToString("C"));
                recordIn = reader.ReadLine();
            }
            reader.Close();
            inFile.Close();
        }
        //repeatedly searches a file to produce 
        //lists of employees who meet a minimum salary requirement
        static void FindEmployees()
        {
            const char DELIM = ',';
            const int END = 999;
            const string FILENAME = "EmployeeData.txt";
            Employee emp = new Employee();
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
                    emp.EmpNum = Convert.ToInt32(fields[0]);
                    emp.Name = fields[1];
                    emp.Salary = Convert.ToDouble(fields[2]);
                    if (emp.Salary >= minSalary)
                        Console.WriteLine("{0,-5}{1,-12}{2,8}", emp.EmpNum,
                            emp.Name, emp.Salary.ToString("C"));
                    recordIn = reader.ReadLine();
                }
                Console.Write("\nEnter minimum salary to find or " +
                    END + " to quit >> ");
                minSalary = Convert.ToDouble(Console.ReadLine());
            }
            reader.Close();  // Error occurs if
            inFile.Close(); //these two statements are reversed
        }
        //Serializable Demonstration
        /// <summary>
        /// writes Person class objects to a file and later reads them 
        /// from the file into the program
        /// </summary>
        static void SerializableDemonstration()
        {
            const int END = 999;
            const string FILENAME = "Data.ser";
            Person emp = new Person();
            FileStream outFile = new FileStream(FILENAME,
                FileMode.Create, FileAccess.Write);
            BinaryFormatter bFormatter = new BinaryFormatter();
            Console.Write("Enter employee number or " + END +
                " to quit >> ");
            emp.EmpNum = Convert.ToInt32(Console.ReadLine());
            while (emp.EmpNum != END)
            {
                Console.Write("Enter last name >> ");
                emp.Name = Console.ReadLine();
                Console.Write("Enter salary >> ");
                emp.Salary = Convert.ToDouble(Console.ReadLine());
                bFormatter.Serialize(outFile, emp);
                Console.Write("Enter employee number or " + END +
                    " to quit >> ");
                emp.EmpNum = Convert.ToInt32(Console.ReadLine());
            }
            outFile.Close();
            FileStream inFile = new FileStream(FILENAME,
                FileMode.Open, FileAccess.Read);
            Console.WriteLine("\n{0,-5}{1,-12}{2,8}\n",
                "Num", "Name", "Salary");
            while (inFile.Position < inFile.Length)
            {
                emp = (Person)bFormatter.Deserialize(inFile);
                Console.WriteLine("{0,-5}{1,-12}{2,8}",
                    emp.EmpNum, emp.Name, emp.Salary.ToString("C"));
            }
            inFile.Close();
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
    
}
