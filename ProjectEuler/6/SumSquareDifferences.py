#Add up all of the squares between start and end
def sumOfSquares(start, end):
    numList = range(start, end)
    sum = 0
    for num in numList:
        sum += num * num
    return sum

#Add up all of the numbers than square that sum
def squareOfSum(start, end):
    numList = range(start, end)
    totalSum = sum(numList)
    return totalSum * totalSum

#Return the square of the sum minus the sum of all the squares
def sumSquareDifference(start, end):
    return  squareOfSum(start, end) - sumOfSquares(start, end)

print sumSquareDifference(1, 101) 