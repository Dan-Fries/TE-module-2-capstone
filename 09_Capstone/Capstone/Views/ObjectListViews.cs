using Capstone.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Capstone.Views
{
    public static class ObjectListViews
    {
        public static void DisplayParks(IList<Park> parks)
        {
            string[] labels = { "Location:", "Established:", "Area:", "Annual Visitors:" };
            foreach (Park park in parks)
            {
                Console.WriteLine();
                Console.WriteLine(park.Name);
                Console.WriteLine($"{labels[0],-24}{park.Location}");
                Console.WriteLine($"{labels[1],-24}{park.EstablishDate:d}");
                Console.WriteLine($"{labels[2],-24}{park.Area:n} sq km");
                Console.WriteLine($"{labels[2],-24}{park.Visitors}");
                Console.WriteLine();
                DisplayParagraphWithWordWrap(park.Description);

            }
        }

        private static void DisplayParagraphWithWordWrap(string text)
        {
            int width = Console.WindowWidth;
            string pattern = @"(?<line>.{1," + width + @"})(?<!\s)(\s+|$)|(?<line>.+?)(\s+|$)";
            var lines = Regex.Matches(text, pattern).Cast<Match>().Select(m => m.Groups["line"].Value);

            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }



    }
}

//string[] headings = { "Number", "Task", "Due Date", "Completed" };
//Console.WriteLine($"{headings[0],6} {headings[1],-30} {headings[2],-15} {headings[3],-10}");
//            foreach (Task task in list)
//            {
//                Console.WriteLine($"{task.Id,6} {task.TaskName,-30} {task.DueDate,-15:d} {task.Completed,-10}");
//            }
