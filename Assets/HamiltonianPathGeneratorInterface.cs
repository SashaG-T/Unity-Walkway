using System;

namespace AssemblyCSharp {
	
	public interface HamiltonianPathGeneratorInterface {

		GraphData generateBase(int width, int height);
		int selectEnd(GraphData graphData);
		int getAdjacentIndex(GraphData graphData, Direction direction, int index);
		Direction oppositeDirection(Direction direction);
		void backbite(GraphData graphData);
		void slaughterbite(GraphData graphData);
		void mergebite(GraphData graphData);

		TileController.TileType getTileType(GraphData graphData, int index);

	}
}

