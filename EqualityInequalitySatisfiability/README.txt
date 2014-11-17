This is an assignment I did in Spring of 2014 for my algorithms and Data Structures Class. I used the programming language Python.

The goal of this assignment was given a massive list of equalities and inequalities, determine whether or not the information clashed. Did some of the inequalities disprove the equalities for example.

First I load in all of the constraints. Inequalities are stored in an 
array of tuples. Equalities are parsed and placed into a 2D array. The 2D array has a list for every node (which has all of its equal nodes). After that I do an iterative Depth First Search on each first node in an inequality looking for the second node. If we find the second node we immediatly return false and output NOT SATISFIABLE. Otherwise we move onto the next inequality. If we make it through the entire thing we output SATISFIABLE