#!/usr/bin/python
import sys


inequalityList = []
variableList = [[]]


#----------HELPER METHODS----------

#setup the 2d variable array with empty arrays
def setupVariableList(line):
    numberOfVars = line.split()[0];
    for num in range(int(numberOfVars)):
        variableList.append([])


#setup the equalitys in the variable array
def addEquality(arg1, arg2):
    val1 = int(arg1[1:])
    val2 = int(arg2[1:])
    variableList[val1].append(val2)
    variableList[val2].append(val1)


#add inequality tuples to the inequality list
def addInequality(arg1, arg2):
    inequalityTuple = (int(arg1[1:]), int(arg2[1:]))
    inequalityList.append(inequalityTuple)


def setupEqualitiesAndInequalities():
    firstLine = True
    for line in sys.stdin:
        if firstLine:
            firstLine = False
            setupVariableList(line)
        elif "==" in line:
            split = line.split()
            addEquality(split[0], split[2])
        elif "!=" in line:
            split = line.split()
            addInequality(split[0], split[2])


#Do a dfs through the variable list, starting at inequality[0] looking for
#inequality[1]
def dfs(inequality):
    visitedNodes = []
    nodesToGoThrough = [inequality[0]]
    while len(nodesToGoThrough) > 0:
        node = nodesToGoThrough.pop()
        if inequality[1] == node or inequality[1] in variableList[node]:
           return False
        visitedNodes.append(node)

        for child in variableList[node]:
            if child not in nodesToGoThrough and child not in visitedNodes:
                nodesToGoThrough.append(child)
    return True

#---------------------------------

def main():

    setupEqualitiesAndInequalities()

    for inequality in inequalityList:
        if not dfs(inequality):
            sys.stdout.write("NOT SATISFIABLE")
            return

    sys.stdout.write("SATISFIABLE")



main()
