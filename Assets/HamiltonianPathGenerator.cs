using System;

namespace AssemblyCSharp {

	public class HamiltonianPathGenerator : HamiltonianPathGeneratorInterface {

		private Random rand = new Random();

		public GraphData generateBase(int width, int height) {

			GraphData graphData = new GraphData (width, height);

			//bool even = true;
			for (int i = 0; i < graphData.size; i++) {
				int connections = 0;

				if (i % width == 0) {
					connections = (int)Direction.Right;
					if ((i / width) % 2 == 0) {
						if (i / width != 0) {
							connections = connections | (int)Direction.Up;
						}
					} else {
						if (i / width < height - 1) {
							connections = connections | (int)Direction.Down;
						}
					}
				} else if (i % width == width - 1) {
					connections = (int)Direction.Left;
					if ((i / width) % 2 == 0) {
						if (i / width < height - 1) {
							connections = connections | (int)Direction.Down;
						} 
					} else {
						if (i / width != 0) {
							connections = connections | (int)Direction.Up;
						}
					}
				} else {
					connections = (int)Direction.Left | (int)Direction.Right;
				}

				graphData.data[i] = connections;
			}

			return graphData;
		}
			
		public int selectEnd(GraphData graphData) {
			
			int index = -1;
			if(graphData != null) {
				if(rand.Next(0, 2) == 0) {
					for(int i = 0; i < graphData.size && index < 0; i++) {
						if(graphData.data[i] == (int)Direction.Up ||
							graphData.data[i] == (int)Direction.Down ||
							graphData.data[i] == (int)Direction.Left ||
							graphData.data[i] == (int)Direction.Right) {

							index = i;
						}
					}
				} else {
					for(int i = graphData.size - 1; i >= 0 && index < 0; i--) {
						if(graphData.data[i] == (int)Direction.Up ||
							graphData.data[i] == (int)Direction.Down ||
							graphData.data[i] == (int)Direction.Left ||
							graphData.data[i] == (int)Direction.Right) {

							index = i;
						}
					}
				}
			}
			return index;
		}

		public int getAdjacentIndex(GraphData graphData, Direction direction, int index) {
			int retVal = -1;
			if (graphData != null) {

				int x = index % graphData.width;
				int y = index / graphData.width;

				switch (direction) {
					case Direction.Up: {
						if (y > 0) {
							retVal = x + (y - 1) * graphData.width;
						}
						break;
					}
					case Direction.Down: {
						if (y < graphData.height - 1) {
							retVal = x + (y + 1) * graphData.width;
						}
						break;
					}
					case Direction.Left: {
						if (x > 0) {
							retVal = (x - 1) + y * graphData.width;
						}
						break;
					}
					case Direction.Right: {
						if (x < graphData.width - 1) {
							retVal = (x + 1) + y * graphData.width;
						}
						break;
					}
				}
			}
			return retVal;
		}

		public Direction oppositeDirection(Direction direction) {

			Direction retVal = Direction.Invalid;

			switch (direction) {
				case Direction.Up: {
					retVal = Direction.Down;
					break;
				}
				case Direction.Down: {
					retVal = Direction.Up;
					break;
				}
				case Direction.Left: {
					retVal = Direction.Right;
					break;
				}
				case Direction.Right: {
					retVal = Direction.Left;
					break;
				}
			}

			return retVal;
		}

		public void backbite(GraphData graphData) {

			int index = selectEnd (graphData);

			if (index > -1) {

				int x = index % graphData.width;
				int y = index / graphData.width;

				int[] viables = new int[3];
				int viableCount = 0;

				int viableConnections = 15 & ~graphData.data[index];
				if((viableConnections & (int)Direction.Up) != 0 && y > 0) {
					if(graphData.data[getAdjacentIndex(graphData, Direction.Up, index)] > 0) {
						viables[viableCount++] = (int)Direction.Up;
					}
				}
				if ((viableConnections & (int)Direction.Down) != 0 && y < graphData.height - 1) {
					if(graphData.data[getAdjacentIndex(graphData, Direction.Down, index)] > 0) {
						viables [viableCount++] = (int)Direction.Down;
					}
				}
				if ((viableConnections & (int)Direction.Left) != 0 && x > 0) {
					if (graphData.data[getAdjacentIndex (graphData, Direction.Left, index)] > 0) {
						viables [viableCount++] = (int)Direction.Left;
					}
				}
				if ((viableConnections & (int)Direction.Right) != 0 && x < graphData.width - 1) {
					if(graphData.data[getAdjacentIndex(graphData, Direction.Right, index)] > 0) {
						viables [viableCount++] = (int)Direction.Right;
					}
				}

				if (viableCount > 0) {

					int dir = viables [rand.Next(0, viableCount)];
					int chosen = getAdjacentIndex (graphData, (Direction)dir, index);
					int badConnection = 0;

					if (chosen > -1) {

						bool notDone = true;
						int next = chosen;
						int lastDir = 0;
						while (notDone) {
							if (((graphData.data[next] & ~lastDir) & (int)Direction.Up) != 0) {
								next = getAdjacentIndex (graphData, Direction.Up, next);
								lastDir = (int)oppositeDirection (Direction.Up);
							} else if (((graphData.data [next] & ~lastDir) & (int)Direction.Down) != 0) {
								next = getAdjacentIndex (graphData, Direction.Down, next);
								lastDir = (int)oppositeDirection (Direction.Down);
							} else if (((graphData.data [next] & ~lastDir) & (int)Direction.Left) != 0) {
								next = getAdjacentIndex (graphData, Direction.Left, next);
								lastDir = (int)oppositeDirection (Direction.Left);
							} else if (((graphData.data [next] & ~lastDir) & (int)Direction.Right) != 0) {
								next = getAdjacentIndex (graphData, Direction.Right, next);
								lastDir = (int)oppositeDirection (Direction.Right);
							} else {
								notDone = false;
								if ((graphData.data [chosen] & (int)Direction.Up) != 0) {
									badConnection = (int)Direction.Up;
								} else if ((graphData.data [chosen] & (int)Direction.Down) != 0) {
									badConnection = (int)Direction.Down;
								} else if ((graphData.data [chosen] & (int)Direction.Left) != 0) {
									badConnection = (int)Direction.Left;
								} else if ((graphData.data [chosen] & (int)Direction.Right) != 0) {
									badConnection = (int)Direction.Right;
								}
								badConnection = graphData.data [chosen] & ~badConnection;
							}
							if (next == index) {

								notDone = false;
								if ((graphData.data [chosen] & (int)Direction.Up) != 0) {
									badConnection = (int)Direction.Up;
								} else if ((graphData.data[chosen] & (int)Direction.Down) != 0) {
									badConnection = (int)Direction.Down;
								} else if ((graphData.data [chosen] & (int)Direction.Left) != 0) {
									badConnection = (int)Direction.Left;
								} else if ((graphData.data [chosen] & (int)Direction.Right) != 0) {
									badConnection = (int)Direction.Right;
								}
							}
						}

						graphData.data [index] = graphData.data [index] | dir;
						graphData.data[chosen] = (graphData.data[chosen] | (int)oppositeDirection ((Direction)dir)) & ~badConnection;
						int adjChosen = getAdjacentIndex (graphData, (Direction)badConnection, chosen);
						graphData.data [adjChosen] = graphData.data [adjChosen] & ~(int)oppositeDirection ((Direction)badConnection);

					}
				}
				
			}

		}

		public void slaughterbite(GraphData graphData) {

			int index = selectEnd (graphData);

			if (index > -1) {

				int connection = graphData.data [index];
				int badConnection = 0;

				int x = index % graphData.width;
				int y = index / graphData.width;

				graphData.data[index] = 0;
				switch ((Direction)connection) {
				case Direction.Up:
					{
						badConnection = (int)Direction.Down;
						y--;
						break;
					}
				case Direction.Down:
					{
						badConnection = (int)Direction.Up;
						y++;
						break;
					}
				case Direction.Left:
					{
						badConnection = (int)Direction.Right;
						x--;
						break;
					}
				case Direction.Right:
					{
						badConnection = (int)Direction.Left;
						x++;
						break;
					}
				}

				int offset = x + y * graphData.width;
				graphData.data[offset] = graphData.data[offset] & ~badConnection;

			}
			
		}

		public void mergebite(GraphData graphData) {

			int index = selectEnd (graphData);

			if (index > -1) {

				//check if end has a detached node beside it. if it does. Merge it back into the path.

				Direction[] validDirections = new Direction[4]; //atmost 4 possible candidates. (4 is rare. must have only 1 empty tile all else full...)
				int validDirectionCount = 0;
				int candidate = getAdjacentIndex (graphData, Direction.Up, index);
				if(candidate > -1) {
					if(graphData.data [candidate] == 0) {
						validDirections [validDirectionCount++] = Direction.Up;
					}
				}
				candidate = getAdjacentIndex (graphData, Direction.Down, index);
				if (candidate > -1) {
					if (graphData.data [candidate] == 0) {
						validDirections [ validDirectionCount++ ] = Direction.Down;
					}
				}
				candidate = getAdjacentIndex (graphData, Direction.Left, index);
				if (candidate > -1) {
					if (graphData.data [candidate] == 0) {
						validDirections [ validDirectionCount++ ] = Direction.Left;
					}
				}
				candidate = getAdjacentIndex (graphData, Direction.Right, index);
				if(candidate > -1) {
					if(graphData.data [candidate] == 0) {
						validDirections [ validDirectionCount++] = Direction.Right;
					}
				}

				//choose a random valid candidate to merge back into the path.
				if (validDirectionCount > 0) {
					
					Direction chosen = validDirections [rand.Next (0, validDirectionCount)]; //retrieve a valid candidate randomly.

					candidate = getAdjacentIndex(graphData, chosen, index);

					//attach candidate to the index.
					graphData.data[candidate] = (int)oppositeDirection(chosen);	//since it's not connected to anything else this is suffice.

					//attach index to candidate.
					graphData.data[index] = graphData.data[index] | (int)chosen;

				}

			}

		}

		public TileController.TileType getTileType(GraphData graphData, int index) {
			TileController.TileType type = TileController.TileType.Blue;
			if (graphData.data [index] == 0) {
				type = TileController.TileType.White;
			}
			return type;
		}

	}
}