﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Helpers
{
	public class SparseMap<T>
	{
		protected readonly Dictionary<int, SparseMapColumn> _column = new Dictionary<int, SparseMapColumn>();
		protected readonly T _defaultValue;

		public SparseMap(T defaultValue = default)
		{
			_defaultValue = defaultValue;
		}

		public IEnumerable<(Point, T)> All(Func<T, bool> predicate = null)
		{
			foreach (var x in _column.Keys)
			{
				var column = _column[x];
				foreach (var y in column.Row.Keys)
				{
					var value = column.Row[y];
					if (predicate == null || predicate(value))
					{
						yield return (Point.From(x, y), value);
					}
				}
			}
		}

		public IEnumerable<Point> AllPoints(Func<T, bool> predicate = null)
		{
			foreach (var x in _column.Keys)
			{
				var column = _column[x];
				foreach (var y in column.Row.Keys)
				{
					if (predicate == null || predicate(column.Row[y]))
					{
						yield return Point.From(x, y);
					}
				}
			}
		}

		public IEnumerable<T> AllValues(Func<T, bool> predicate = null)
		{
			foreach (var x in _column.Keys)
			{
				var column = _column[x];
				foreach (var y in column.Row.Keys)
				{
					var value = column.Row[y];
					if (predicate == null || predicate(value))
					{
						yield return value;
					}
				}
			}
		}

		public IEnumerable<Point> AllArea()
		{
			var (min, max) = Area();
			for (var x = min.X; x <= max.X; x++)
			{
				for (var y = min.Y; y <= max.Y; y++)
				{
					yield return Point.From(x, y);
				}
			}
		}

		public Point FirstOrDefault(Func<T, bool> predicate)
		{
			return AllPoints(predicate).FirstOrDefault();
		}

		public T this[Point pos]
		{
			get => _column.TryGetValue(pos.X, out var col) ? col[pos.Y] : _defaultValue;
			set => this[pos.X][pos.Y] = value;
		}

		public bool Exists(Point pos)
		{
			return _column.TryGetValue(pos.X, out var col) && col.Exists(pos.Y);
		}

		public SparseMapColumn this[int x]
		{
			get
			{
				if (!_column.ContainsKey(x))
				{
					_column[x] = new SparseMapColumn(_defaultValue);
				}
				return _column[x];
			}
		}

		public (Point, Point) Area()
		{
			var points = AllPoints().ToArray();
			if (points.Length == 0)
			{
				return (Point.From(0, 0), Point.From(0, 0));
			}
			var min = Point.From(points.Min(z => z.X), points.Min(z => z.Y));
			var max = Point.From(points.Max(z => z.X), points.Max(z => z.Y));
			return (min, max);
		}

		public IEnumerable<Point> Span()
		{
			var (min, max) = Area();
			yield return min;
			yield return Point.From(max.X, min.Y);
			yield return max;
			yield return Point.From(min.X, max.Y);
		}

		
		public string[] Render(Func<Point, T, char> rendering)
		{
			var (min, max) = Area();
			return Enumerable.Range(min.Y, max.Y- min.Y + 1)
				.Select(y => Enumerable.Range(min.X, max.X - min.X + 1)
					.Select(x => rendering(Point.From(x, y), this[x][y]))
					.ToArray()
				)
				.Select(ch => new string(ch))
				.ToArray();
		}

		public void ConsoleWrite(Func<Point, T, char> rendering)
		{
			foreach (var line in Render(rendering))
			{
				Console.WriteLine(line);
			}
		}

		public class SparseMapColumn
		{
			internal readonly Dictionary<int, T> Row = new Dictionary<int, T>();
			private readonly T _defaultValue;

			public SparseMapColumn(T defaultValue)
			{
				_defaultValue = defaultValue;
			}

			public T this[int y]
			{
				get => Row.ContainsKey(y) ? Row[y] : _defaultValue;
				set => Row[y] = value;
			}

			public bool Exists(int y)
			{
				return Row.ContainsKey(y);
			}
		}
	}
}
