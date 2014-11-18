#Returns true if the num divides evenly into any member of the list
def checkIfNumDividesIntoAnyMemberInList(num, numList):
    if len(numList) == 0:
        return False
    for n in numList:
        if (n % num == 0):
            return True
    return False

#Eliminate any values that evenly divide into other values (like we don't need 3 and 6, since everything 
#divisable by 6 is divisable by 3)
def makeShortenedList(min, max):
    fullList = (range(min, max))
    fullList.reverse()
    shortList = []        
    for num in fullList:
        if not checkIfNumDividesIntoAnyMemberInList(num, shortList):
            shortList.append(num)
    return shortList

#returns true if every number in numlist divides evenly into num
def checkIfAllMembersOfListDivideInto(num, numList):
    for n in numList:
        if not num % n == 0:
            return False
    return True

#finds the smallest number that every number between min (inclusive) and max(inclusive) divides
#evenly into
def findSmallestMultiple(min, max):
    shortenedList = makeShortenedList(min, max)
    if(len(shortenedList) == 0):
        return -1
    if(len(shortenedList) == 1):
        return shortenedList[0]
    currentNum = shortenedList[0] * shortenedList[1]
    haveFoundSmallestMultiple = False
    while not haveFoundSmallestMultiple:
        if checkIfAllMembersOfListDivideInto(currentNum, shortenedList):
            return currentNum
        currentNum += shortenedList[0] * shortenedList[1]

print findSmallestMultiple(1, 21)