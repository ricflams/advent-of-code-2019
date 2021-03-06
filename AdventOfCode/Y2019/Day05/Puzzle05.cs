﻿using AdventOfCode.Helpers;
using AdventOfCode.Helpers.Puzzles;
using AdventOfCode.Y2019.Intcode;
using System.Linq;

namespace AdventOfCode.Y2019.Day05
{
	internal class Puzzle : Puzzle<long, long>
	{
		public static Puzzle Instance = new Puzzle();
		public override string Name => "Sunny with a Chance of Asteroids";
		public override int Year => 2019;
		public override int Day => 5;

		public void Run()
		{
			Run("input").Part1(7566643).Part2(9265694);
		}

		protected override long Part1(string[] input)
		{
			var intcode = input[0];
			var result = new Engine()
				.WithMemory(intcode)
				.WithInput(1)
				.Execute()
				.Output.TakeAll()
				.SkipWhile(x => x == 0)
				.First();
			return result;
		}

		protected override long Part2(string[] input)
		{
			var intcode = input[0];
			var result = new Engine()
				.WithMemory(intcode)
				.WithInput(5)
				.Execute()
				.Output.TakeAll()
				.SkipWhile(x => x == 0)
				.First();
			return result;
		}
	}
}