using System;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

namespace Linq
{
    class Program
    {
        static void Main(string[] args)
        {
            var people = ReadPeopleFromJsonFile();

            var firstPerson = people.FirstOrDefault();
            Console.WriteLine(firstPerson);
            var lastPerson = people.LastOrDefault();
            Console.WriteLine(lastPerson);

            //FIRST PERSON STARTS WITH LETTER A
            var firstPersonStartsWithA = people.FirstOrDefault( p => p.FirstName.StartsWith("A"));
            Console.WriteLine(firstPersonStartsWithA);

            var singlePerson = people.SingleOrDefault( p=> p.Id == 10004 );
            Console.WriteLine(singlePerson);

            //MAX AVERAGE
            var maxSalary = people.Max( p=> p.Salary );
            Console.WriteLine(maxSalary);

            //MIN AVERAGE
            var minSalary = people.Min( p => p.Salary );
            Console.WriteLine(minSalary);

            //AVERAGE
            var averageSalary = people.Average( p => p.Salary );
            Console.WriteLine(averageSalary);

            //ANY
            if(!people.Any()){
                Console.WriteLine("Is empry");
            }else{ Console.WriteLine("There is somebody"); }

            //ORDER
            var peopleOrderByAge = people.OrderBy( p => p.BirthDate ).ThenBy( p => p.FirstName ).ToArray();
            // foreach(Person p in peopleOrderByAge){
            //     Console.WriteLine(p);
            // }

            //TAKE AND SKIP
            var firstTenPeople = people.Take(10);
            var skipTenPeople = people.Skip(10);

            //PAGINATION
            int pageSize = 10;
            int pageNumber = 1;

            var peoplePaginated = people.Skip( (pageNumber -1) * pageSize).Take(pageSize);
            foreach(Person p in peoplePaginated){
                Console.WriteLine(p);
            }

            //WHERE
            Console.WriteLine("WHERE .........................................");
            var filteredPeople = people.Where( p => p.BirthDate > new DateTime(1990, 1, 1) && p.Salary > 14000);
            foreach( var item in filteredPeople){
                Console.WriteLine(item);
            }

            //Grouping
            var peopleByCity = people.GroupBy( p => p.City );
            foreach(var item in peopleByCity){
                Console.WriteLine(item.Key);
                foreach(var person in item){
                    Console.WriteLine(person);
                }
            }

            Console.WriteLine("QUERY SINTAX .........................................");
            //Query syntax
            var peopleWithHightSalaries = (
                from p in people 
                where p.Salary > 14000
                orderby p.Id descending 
                select p
            );

            //PROJECTION
            Console.WriteLine("RETURN ONLY ID .........................................");
            var oldersPeopleIds = people.Where( p => p.BirthDate < new DateTime(1990,1,1)).Select( p => p.Id);
            foreach(var item in oldersPeopleIds){
                Console.WriteLine(item);
            }

        }

        static Person[] ReadPeopleFromJsonFile(){

            using(var reader = new StreamReader("people.json")){
                string jsonData = reader.ReadToEnd();
                var people = JsonConvert.DeserializeObject<Person[]>(jsonData);
                return people;
            }
        }
    }

    public class Person{

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public decimal Salary { get; set; }
        public DateTime BirthDate { get; set; }

        public override string ToString()
        {
            return $" {Id} {FirstName} {LastName} | {City} | {BirthDate.ToShortDateString()} | ${Salary}";
        }
    }
}
