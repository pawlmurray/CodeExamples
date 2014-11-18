#Returns true if any number in the list divides into num
def checkIfAnyNumberInTheListDividesIntoNum(num, numList):
    if len(numList) == 0:
        return False
    for n in numList:
        if (num % n == 0):
            return True
    return False

#find's the nth prime number
def findNthPrimeNumber(n):
    primes = []
    currentNum = 2
    searchingForNthPrime = True
    while searchingForNthPrime:
        if not checkIfAnyNumberInTheListDividesIntoNum(currentNum, primes):
            primes.append(currentNum)
            if len(primes) == n:
                return primes[-1]
        currentNum +=1

print findNthPrimeNumber(10001)
