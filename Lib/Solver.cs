using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AdventOfCode {

    interface Solver {
        string Name { get; }
        IEnumerable<object> Solve(string input);
    }

    static class SolverExtensions {
        public static string DayName(this Solver solver) {
            return $"Day {solver.Day()}";
        }

        public static int Year(this Solver solver) {
            return Year(solver.GetType());
        }

        public static int Year(Type t) {
            return int.Parse(t.FullName!.Split('.')[1][1..]);
        }
        public static int Day(this Solver solver) {
            return Day(solver.GetType());
        }

        public static int Day(Type t) {
            return int.Parse(t.FullName!.Split('.')[2][3..]);
        }

        public static string WorkingDir(int year) {
            return Path.Combine(year.ToString());
        }

        public static string WorkingDir(int year, int day) {
            return Path.Combine(WorkingDir(year), "Day" + day.ToString("00"));
        }

        public static string WorkingDir(this Solver solver) {
            return WorkingDir(solver.Year(), solver.Day());
        }

        public static SplashScreen SplashScreen(this Solver solver) {
            var tsplashScreen = Assembly.GetEntryAssembly()!.GetTypes()
                 .Where(t => t.GetTypeInfo().IsClass && typeof(SplashScreen).IsAssignableFrom(t))
                 .Single(t => Year(t) == solver.Year());
            return (SplashScreen)Activator.CreateInstance(tsplashScreen)!;
        }
    }
}