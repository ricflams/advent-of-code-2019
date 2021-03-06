﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Helpers
{
    public class WeightedGraph<T>
    {
		public Dictionary<T, Vertex> Vertices { get; } = new Dictionary<T, Vertex>();
		public Vertex Root => Vertices.Values.FirstOrDefault();

		public class VertexByName : Dictionary<string, Vertex>
		{
			public new Vertex this[string name]
			{
				get => TryGetValue(name, out var vertex) ? vertex : null;
				set => base[name] = value;
			}
		}
		[DebuggerDisplay("{ToString()}")]
		public class Vertex
		{
			public T Value { get; set; }
			public IDictionary<Vertex, int> Edges { get; } = new Dictionary<Vertex, int>();

			//// Always present, for ease-of-use in graph-searches
			//public int Distance { get; set; }
			//public bool Visited { get; set; }

			public override string ToString() => $"{Value?.ToString() ?? ""}";
		}

		public void WriteAsGraphwiz()
		{
			Console.WriteLine("digraph {");
			foreach (var v in Vertices.Values)
			{
				foreach (var e in v.Edges)
				{
					Console.WriteLine($"  \"{v.Value}\" -> \"{e.Key.Value}\" [label=\"{e.Value}\"]");
				}
			}
			Console.WriteLine("}");
		}

		public void AddVertices(T val1, T val2, int distance)
		{
			var v1 = GetOrCreateVertex(val1);
			var v2 = GetOrCreateVertex(val2);
			AddEdge(v1, v2, distance);
		}

		private Vertex GetOrCreateVertex(T key)
		{
			if (!Vertices.TryGetValue(key, out var v))
			{
				v = new Vertex { Value = key };
				Vertices[key] = v;
			}
			return v;
		}

		public static void AddEdge(Vertex v1, Vertex v2, int weight)
		{
			v1.Edges.Add(v2, weight);
			v2.Edges.Add(v1, weight);
		}

		public int TspShortestDistanceBruteForce()
		{
			var vertices = Vertices.Values.ToArray();
			var N = vertices.Length;
			var mindistance = int.MaxValue;
			foreach (var perm in MathHelper.AllPermutations(N))
			{
				var visits = perm.Select(i => vertices[i]).ToArray();
				var distance = 0;
				for (var i = 0; i < N - 1; i++)
				{
					distance += visits[i].Edges[visits[i + 1]];
				}
				if (distance < mindistance)
				{
					mindistance = distance;
				}
			}
			return mindistance;
		}

		public int TspLongestDistanceBruteForce()
		{
			var vertices = Vertices.Values.ToArray();
			var N = vertices.Length;
			var maxdistance = 0;
			foreach (var perm in MathHelper.AllPermutations(N))
			{
				var visits = perm.Select(i => vertices[i]).ToArray();
				var distance = 0;
				for (var i = 0; i < N - 1; i++)
				{
					distance += visits[i].Edges[visits[i + 1]];
				}
				if (distance > maxdistance)
				{
					maxdistance = distance;
				}
			}
			return maxdistance;
		}
	}
}
