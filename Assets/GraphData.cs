using UnityEngine;
using System;

namespace AssemblyCSharp {
	
	public class GraphData {
		public int width;
		public int height;
		public int size;
		public int[] data;

		public GraphData(int width, int height) {
			this.width = width;
			this.height = height;
			this.size = width * height;
			data = new int[size];
		}

		public Vector2 convertIndexToVector2(int index) {
			return new Vector2 ((float)(index % width), (float)(index / width));
		}
	}
}

