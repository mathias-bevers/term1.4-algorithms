using System;
using System.Drawing;
using GXPEngine;
using GXPEngine.OpenGL;
using Mathias;
using Mathias.Agents;
using Mathias.Utilities;

/**
 * This is the main 'game' for the Algorithms Assignment that accompanies the Algorithms course.
 * 
 * Read carefully through the assignment that you are currently working on
 * and then through the code looking for all pointers & TODO's that you have to implement.
 * 
 * The course is 6 weeks long and this is the only assignment/code that you will get,
 * split into 3 major parts (see below). This means that you have three 2 week sprints to
 * work on your assignments.
 */
internal class AlgorithmsAssignment : Game
{
	private const int MIN_ROOM_SIZE = 5;

	//common settings
	private const int SCALE = 20;
	public static Grid Grid { get; private set; }
	public static Random Random { get; private set; }
	public static AlgorithmsAssignment Instance { get; private set; }

	//Required for assignment 1
	private DungeonBase _dungeon;
	private NodeGraphAgentBase _agent;

	//Required for assignment 2
	private NodeGraphBase _graph;

	//Required for assignment 3
	private PathFinderBase _pathFinder = null;
	private TiledViewBase _tiledView;

	//common settings
	private const int SCALE = 20;
	private const int MIN_ROOM_SIZE = 5;
	public AlgorithmsAssignment() : base(800, 600, false)
	{
		Instance = game as AlgorithmsAssignment;

		GL.ClearColor(1, 1, 1, 1);
		GL.glfwSetWindowTitle("Algorithms Game");
		Random = new Random(1);

		Grid grid = new(width, height, SCALE);
		Grid = grid;
		Size size = new(width / SCALE, height / SCALE);

		#region part 1

		_dungeon = new Dungeon(size, GradeType.Good);
		if (_dungeon != null)
		{
			_dungeon.scale = SCALE;
			_dungeon.Generate(MIN_ROOM_SIZE);
		}

		#endregion

		#region part 2

		_graph = new NodeGraph(_dungeon, NodeGraph.Level.Low);
		_graph?.Generate();

		_agent = new NodeGraphAgent(_graph, GradeType.Good);
		
		if ((_agent as NodeGraphAgent)?.gradeType == GradeType.Good)
		{
			_tiledView = new TiledDungeonView(_dungeon, TileType.GROUND);
			_tiledView?.Generate();
		}
		

		

		#endregion

		//////////////////////////////////////////////////////////////
		//Assignment 2.3 Excellent (Optional) LowLevelDungeonNodeGraph
		//
		//TODO: Comment out the HighLevelDungeonNodeGraph above, and implement the LowLevelDungeonNodeGraph 

		/////////////////////////////////////////////////////////////////////////////////////////
		/// ASSIGNMENT 3 : PathFinding and PathFindingAgents
		///							
		/// SKIP THIS BLOCK UNTIL YOU'VE FINISHED ASSIGNMENT 2 AND ASKED FOR TEACHER FEEDBACK !

		//////////////////////////////////////////////////////////////////////////
		//Assignment 3.1 Sufficient (Mandatory) - Recursive Pathfinding
		//
		//TODO: Study assignment 3.1 on blackboard
		//TODO: Study the PathFinder class
		//TODO: Study the SamplePathFinder class and try it out
		//TODO: Comment out the SamplePathFinder, implement a RecursivePathFinder and uncomment it below

		//_pathFinder = new SamplePathFinder(_graph);
		_pathFinder = new PathFinder(_graph);

		

		//TODO: Implement a PathFindingAgent that uses one of your pathfinder implementations (should work with any pathfinder implementation)
		_agent = new PathFindingAgent(_graph, _pathFinder);

		/////////////////////////////////////////////////
		//Assignment 3.2 Good & 3.3 Excellent (Optional)
		//
		//There are no more explicit TODO's to guide you through these last two parts.
		//You are on your own. Good luck, make the best of it. Make sure your code is testable.
		//For example for A*, you must choose a setup in which it is possible to demonstrate your 
		//algorithm works. Find the best place to add your code, and don't forget to move the
		//PathFindingAgent below the creation of your PathFinder!

		//------------------------------------------------------------------------------------------
		/// REQUIRED BLOCK OF CODE TO ADD ALL OBJECTS YOU CREATED TO THE SCREEN IN THE CORRECT ORDER
		/// LOOK BUT DON'T TOUCH :)

		if (grid != null) { AddChild(grid); }

		if (_dungeon != null) { AddChild(_dungeon); }

		if (_tiledView != null) { AddChild(_tiledView); }

		if (_graph != null) { AddChild(_graph); }

		if (_pathFinder != null)
		{
			AddChild(_pathFinder);				//pathfinder on top of that
		}

		if (_graph != null)
		{
			AddChild(new NodeLabelDrawer(_graph));	//node label display on top of that
		}

		if (_agent != null)
		{
			AddChild(_agent); //and last but not least the agent itself
		}

		/////////////////////////////////////////////////
		//The end!
		////
	}
}