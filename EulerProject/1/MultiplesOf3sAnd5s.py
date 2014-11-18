#get all of the multiples of threes and add them up
def sumOfMultiplesOfThrees(max):
    currentSum = 0
    threeCounter = 3
    while(threeCounter < max):
        currentSum+= threeCounter
        threeCounter += 3
    return currentSum

    #Get all of the multiples of fives and add them up, if they are
#evenly divisible by three just leave them out
def sumOfMultiplesOfFivesWithoutThrees(max):
    currentSum = 0
    fiveCounter = 5
    while(fiveCounter < max):
        if not fiveCounter % 3 == 0:
            currentSum += fiveCounter
        fiveCounter += 5
    return currentSum

totalSum = sumOfMultiplesOfThrees(1000) + sumOfMultiplesOfFivesWithoutThrees(1000)
print totalSum