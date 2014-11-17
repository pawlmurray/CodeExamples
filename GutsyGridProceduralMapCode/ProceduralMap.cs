using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using GutsyGridAlpha.Battle;
using GutsyGridAlpha.Battle.Actors;
using GutsyGridAlpha.Overworld.Nodes;
using GutsyGridAlpha.TesterLibrary;
using GutsyGridAlpha.Scenes;


namespace GutsyGridAlpha.Overworld
{
    class ProceduralMap : Map
    {
        //The amount of nodes in this map
        int count = 0;

        const int PIXEL_WIDTH_BETWEEN_NODE = 150;
        const int PIXEL_HEIGHT_BETWEEN_NODE = 150;

        // Pixel start positions
        const int MAP_START_X = 125;
        const int MAP_START_Y = 100;

        // booleans used to determine if we need to keep moving east or south
        Boolean isEndEast = false;
        Boolean isEndSouth = false;

        // The NODE width and height of the map.
        int mapWidth;
        int mapHeight;

        const int BOSS_ID = 13;
        const int BOSS_BATTLE_ID = 12;

        public ProceduralMap (int mapWidth, int mapHeight, ContentManager content) 
            : base(content)
        {
            this.mapWidth = mapWidth;
            this.mapHeight = mapHeight;
            MakeNodes();
            this.startingNode = nodes[0];
        }

        // Create all of the nodes for the map.
        private void MakeNodes()
        {
            // First create the start and endPoints.
            List<Point> startAndEndPoints = MakeStartAndEndPoints();
            Point startPoint = startAndEndPoints[0];
            Point endPoint = startAndEndPoints[1];

            // Figure out the end points relationship to the start point.
            isEndEast = startPoint.X < endPoint.X;
            isEndSouth = startPoint.Y < endPoint.Y;

            // Create the startNode and the endNode. 
            BattleNode startNode = new BattleNode(0, startPoint, content, BattleCreator.CreateRandomBattleSceneForDifficulty(0, mapWidth, mapHeight, content));
            BossNode endNode = new BossNode(BOSS_ID, endPoint, content, BattleCreator.CreateBattleSceneFromBattleId(BOSS_BATTLE_ID, content));
            
            // Add the startNode to the beginning.
            nodes.Add(0, startNode);
            count++;

            // Create the rest of the map
            MakeNextNodes(startNode, endNode, 1);

            // Connect the last node we made to the endNode.
            ConnectFinalNodeToEnd(endNode);

            // Lastly go back and see if we have any candidates for making a key node and a locked node.
            TryToMakeKeyAndLockedNode(startNode, endNode);
        }


        // Create the start and end points. The start poing is chosen from one of the four corner points of the map.
        // The end point is ALWAYS chosen to be the corner point that is diagonally across from the start point.
        private List<Point> MakeStartAndEndPoints()
        {
            Point topLeft = new Point(MAP_START_X, MAP_START_Y);
            Point topRight = new Point(MAP_START_X + PIXEL_WIDTH_BETWEEN_NODE * (mapWidth - 1), MAP_START_Y);
            Point bottomLeft = new Point(MAP_START_X, MAP_START_Y + PIXEL_HEIGHT_BETWEEN_NODE * (mapHeight - 1));
            Point bottomRight = new Point(MAP_START_X + PIXEL_WIDTH_BETWEEN_NODE * (mapWidth - 1), 
                                          MAP_START_Y + PIXEL_HEIGHT_BETWEEN_NODE * (mapHeight - 1));
            List<Point> possiblePoints = new List<Point>{topLeft, topRight, bottomLeft, bottomRight};
            Point startPoint = possiblePoints[GutsyMain.rand.Next(possiblePoints.Count)];
            possiblePoints.Remove(startPoint);
            Point endPoint = new Point(-1, -1);
            foreach(Point point in possiblePoints)
            {
                if(point.X != startPoint.X && point.Y != startPoint.Y)
                {
                    endPoint = point;
                    break;
                }
            }
            List<Point> points = new List<Point>() { startPoint, endPoint } ;
            return points;
        }

        // Create the net series of nodes
        private void MakeNextNodes(Node previous, Node destination, int currentDistance)
        {
            // If we can add a ndoe horizontally and vertically do one of three things randomly.
            // 1. Add a horizontal node
            // 2. Add a vertical node
            // 3. Add both a horizontal and vertical node.
            if (CanKeepGoingHorizontal(previous, destination) &&
                CanKeepGoingVertical(previous, destination))
            {
                int randDirection = GutsyMain.rand.Next(3);
                if (randDirection == 0)
                {
                    AddNodeHorizontal(previous, destination, isEndEast, currentDistance);
                }
                else if (randDirection == 1)
                {
                    AddNodeVertical(previous, destination, isEndSouth, currentDistance);
                }
                else
                {
                    AddNodeHorizontal(previous, destination, isEndEast, currentDistance);
                    AddNodeVertical(previous, destination, isEndSouth, currentDistance);
                }
            }
            else if (CanKeepGoingHorizontal(previous, destination))
            {
                AddNodeHorizontal(previous, destination, isEndEast, currentDistance);
            }
            else if (CanKeepGoingVertical(previous, destination))
            {
                AddNodeVertical(previous, destination, isEndSouth, currentDistance);
            }
        }

        // Returns whether or not the mapGenerator can keep generating nodes in the horizontal direction.
        private Boolean CanKeepGoingHorizontal(Node previous, Node destination)
        {
            return (isEndEast && previous.GetLocation().X < destination.GetLocation().X) ||
                   (!isEndEast && previous.GetLocation().X > destination.GetLocation().X);
        }

        // Returns whether or not the mapGenerator can keep generating nodes in the vertical direction..
        private Boolean CanKeepGoingVertical(Node previous, Node destination)
        {
            return (isEndSouth && previous.GetLocation().Y < destination.GetLocation().Y) ||
                   (!isEndSouth && previous.GetLocation().Y > destination.GetLocation().Y);
        }


        // Add a horizontal node and continue.
        private void AddNodeHorizontal(Node previous, Node destination, Boolean isEast, int currentDistance)
        {
            int xPos = previous.GetLocation().X;
            if (isEast)
                xPos += PIXEL_WIDTH_BETWEEN_NODE;
            else
                xPos -= PIXEL_WIDTH_BETWEEN_NODE;
            BattleNode nextNode = new BattleNode(count, new Point(xPos, previous.GetLocation().Y),
                                                 content, BattleCreator.CreateRandomBattleSceneForDifficulty(currentDistance, mapWidth, mapHeight, content));
            if (!DoesNodeOverlap(nextNode, destination))
            {
                AddAndConnectNewNode(previous, nextNode);
                MakeNextNodes(nextNode, destination, currentDistance + 1);
            }
        }

        // Add a vertical node and continue.
        private void AddNodeVertical(Node previous, Node destination, Boolean isSouth, int currentDistance)
        {
            int yPos = previous.GetLocation().Y;
            if (isSouth)
                yPos += PIXEL_HEIGHT_BETWEEN_NODE;
            else
                yPos -= PIXEL_HEIGHT_BETWEEN_NODE;
            BattleNode nextNode = new BattleNode(count, new Point(previous.GetLocation().X, yPos),
                                                  content, BattleCreator.CreateRandomBattleSceneForDifficulty(currentDistance, mapWidth, mapHeight, content));
            if (!DoesNodeOverlap(nextNode, destination))
            {
                AddAndConnectNewNode(previous, nextNode);
                MakeNextNodes(nextNode, destination, currentDistance + 1);
            }
        }

        // Check whether or not we already created a node in this location.
        private Boolean DoesNodeAlreadyExistInNodes(Node node)
        {
            foreach (Node n in nodes.Values)
            {
                if (n.GetLocation().Equals(node.GetLocation()))
                    return true;
            }
            return false;
        }

        // Checks whether or not Node node is in the smae location as another node in the list
        // or the destination node.
        private Boolean DoesNodeOverlap(Node node, Node destination)
        {
            return node.GetLocation().Equals(destination.GetLocation()) ||
                   DoesNodeAlreadyExistInNodes(node);
        }

        // Add the new node to nodes and connect it to the previous node. Connecting a node sets up the relationship
        // that allows the player to move between them and renders a badly drawn path.
        private void AddAndConnectNewNode(Node previous, Node newNode)
        {
            // If we moved horizontally last time
            if (previous.GetLocation().Y == newNode.GetLocation().Y)
            {
                nodes.Add(count, newNode);
                count++;
                if (previous.GetLocation().X < newNode.GetLocation().X)
                {
                    ConnectEast(previous, newNode);
                    ConnectWest(newNode, previous);
                }
                else
                {
                    ConnectEast(newNode, previous);
                    ConnectWest(previous, newNode);
                }
            }
            // If we moved verticall last time
            else
            {
                nodes.Add(count, newNode);
                count++;
                if (previous.GetLocation().Y < newNode.GetLocation().Y)
                {
                    ConnectSouth(previous, newNode);
                    ConnectNorth(newNode, previous);
                }
                else
                {
                    ConnectSouth(newNode, previous);
                    ConnectNorth(previous, newNode);
                }
            }
        }

        // Connects final node(s) we generated to the end node. 
        private void ConnectFinalNodeToEnd(Node endNode)
        {
            int nodesAdjacent = 0;
            List<Node> adjacentNodes = new List<Node>();
            // Grab all the adjacent nodes to the end node.
            foreach(Node node in nodes.Values)
            {
                if (node.GetLocation().X == endNode.GetLocation().X &&
                    CanKeepGoingVertical(node, endNode) && 
                    Math.Abs(node.GetLocation().Y - endNode.GetLocation().Y) == PIXEL_HEIGHT_BETWEEN_NODE)
                {
                    adjacentNodes.Add(node);
                    nodesAdjacent++;
                }
                else if(node.GetLocation().Y == endNode.GetLocation().Y &&
                        CanKeepGoingHorizontal(node, endNode) &&
                        Math.Abs(node.GetLocation().X - endNode.GetLocation().X) == PIXEL_WIDTH_BETWEEN_NODE)
                {
                    adjacentNodes.Add(node);
                    nodesAdjacent++;
                }
            }

            // If there's only one adjacent node we need to use that one (so we can get to the end node).
            if (nodesAdjacent == 1)
            {
                AddAndConnectNewNode(adjacentNodes[0], endNode);
            }
            // If there are two adjacent nodes we can randomly pick between three situations. 1. Connect the first adjacent. 2. Connect the second.
            // 3. Connect both
            else if (nodesAdjacent == 2)
            {
                int randomChance = GutsyMain.rand.Next(3);
                if (randomChance == 0)
                {
                    AddAndConnectNewNode(adjacentNodes[0], endNode);
                }
                else if (randomChance == 1)
                {
                    AddAndConnectNewNode(adjacentNodes[1], endNode);
                }
                else
                {
                    AddAndConnectNewNode(adjacentNodes[0], endNode);
                    AddAndConnectNewNode(adjacentNodes[1], endNode);
                }
            }
        }

        //Returns all nodes between the startNode and the destination Node.
        // Will return an empty list if the nodes aren't actually connected
        private List<Node> GetAllNodesInBetween(Node startNode, Node destinationNode)
        {
            List<Node> nodesInBetween = new List<Node>();
            Boolean isDestinationEast = startNode.GetLocation().X < destinationNode.GetLocation().X;
            Boolean isDestinationSouth = startNode.GetLocation().Y < destinationNode.GetLocation().Y;
            Node currentNode = startNode;
            Boolean hasHorizontalNeighbor;
            Boolean hasVerticalNeighbor;
            // Basically traverse through the starting node until we get to the end node.
            while (!currentNode.GetLocation().Equals(destinationNode.GetLocation()))
            {
                hasHorizontalNeighbor = ((isDestinationEast && currentNode.HasEast()) || (!isDestinationEast && currentNode.HasWest())) &&
                                        currentNode.GetLocation().X != destinationNode.GetLocation().X;
                hasVerticalNeighbor = ((isDestinationSouth && currentNode.HasSouth()) || (!isDestinationSouth && currentNode.HasNorth())) && 
                                      currentNode.GetLocation().Y != destinationNode.GetLocation().Y;
                
                // If our currentNode has a branching path(a horizontal and vertical neighbor) we need to go down both of these paths.
                if (hasHorizontalNeighbor && hasVerticalNeighbor)
                {
                    nodesInBetween.Add(currentNode);
                    if (isDestinationEast)
                    {
                        nodesInBetween.AddRange(GetAllNodesInBetween(currentNode.GetEast(), destinationNode));
                    }
                    else if(currentNode.GetLocation().X != destinationNode.GetLocation().X)
                    {
                        nodesInBetween.AddRange(GetAllNodesInBetween(currentNode.GetWest(), destinationNode));
                    }
                    if (isDestinationSouth)
                    {
                        nodesInBetween.AddRange(GetAllNodesInBetween(currentNode.GetSouth(), destinationNode));
                    }
                    else if(currentNode.GetLocation().Y != destinationNode.GetLocation().Y)
                    {
                        nodesInBetween.AddRange(GetAllNodesInBetween(currentNode.GetNorth(), destinationNode));
                    }
                    return nodesInBetween;
                }
                // If we have a horizontal neighbor go to it.
                else if (hasHorizontalNeighbor)
                {
                    nodesInBetween.Add(currentNode);
                    if (isDestinationEast)
                        currentNode = currentNode.GetEast();
                    else
                        currentNode = currentNode.GetWest();
                        
                }
                // if we have a vertical neighbor go to it.
                else if (hasVerticalNeighbor)
                {
                    nodesInBetween.Add(currentNode);
                    if (isDestinationSouth)
                        currentNode = currentNode.GetSouth();
                    else
                        currentNode = currentNode.GetNorth();
                }
                // Else we've hit dead end. If we're at our destination return the list. Otherwise return an empty list.
                else
                {
                    if (nodesInBetween.Contains(destinationNode))
                        return nodesInBetween;
                    return new List<Node>();
                }              
            }
            nodesInBetween.Add(currentNode);
            return nodesInBetween;
        }

        // A key and locked node are two nodes. One node has a key that a player can pick up. The other is a locked node that 
        // the player cannot access until they've gotten the key.
        // When spawning these we must look for nodes OFF of the main path to the end node. In other words key and locked nodes
        // can only spawn on completely OPTIONAL nodes.
        private void TryToMakeKeyAndLockedNode(Node startNode, Node endNode)
        {
            HashSet<Node> pathFromStartToEnd = new HashSet<Node>(GetAllNodesInBetween(startNode, endNode));
            List<KeyValuePair<int, Node>> possibleNodesForKeysAndLocks = new List<KeyValuePair<int, Node>>();
            // get all nodes that are NOT on the main path and are a dead end.
            foreach (KeyValuePair<int, Node> nodePair in nodes)
            {
                if (!pathFromStartToEnd.Contains(nodePair.Value) && nodePair.Value.IsDeadEnd())
                    possibleNodesForKeysAndLocks.Add(nodePair);
            }

            // If we've got at least two nodes that meet the qualificaions we can make a key node and a lock node.
            // (We never want just a key node or a lock node so we need two!)
            if (possibleNodesForKeysAndLocks.Count >= 2)
            {
                KeyValuePair<int, Node> keyNodePair = possibleNodesForKeysAndLocks[GutsyMain.rand.Next(possibleNodesForKeysAndLocks.Count)];
                possibleNodesForKeysAndLocks.Remove(keyNodePair);
                KeyValuePair<int, Node> lockNodePair = possibleNodesForKeysAndLocks[GutsyMain.rand.Next(possibleNodesForKeysAndLocks.Count)];

                KeyNode keyNode = new KeyNode(keyNodePair.Value.GetId(), keyNodePair.Value.GetLocation(), GutsyMain.content);
                LockedShopNode lockedNode = new LockedShopNode(lockNodePair.Value.GetId(), lockNodePair.Value.GetLocation(), GutsyMain.content);

                // Remove the old nodes that were there and replace them with the keyNode and the lockedNode.
                keyNode.CopyNeighbors(keyNodePair.Value);
                lockedNode.CopyNeighbors(lockNodePair.Value);
                nodes.Remove(keyNodePair.Key);
                nodes.Remove(lockNodePair.Key);
                nodes.Add(keyNodePair.Key, keyNode);
                nodes.Add(lockNodePair.Key, lockedNode);
            }
        }
    }
}
