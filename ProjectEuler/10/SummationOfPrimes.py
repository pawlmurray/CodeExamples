#Cool way of calculating all of the primes less then n.
#Essentailly we make a blank dictionary with keys being
#the number and values representing whether or not it is prime.
#After that we iterate through looking for a number that is still identified
#as prime. Once we discover a prime we continually multiply that number
#by increasing multiples (starting at a multiple of itself), marking those
#products as not prime. 

#general concept came from: http://en.wikipedia.org/wiki/Sieve_of_Eratosthenes
def sieveOfEratostehnes(n):
    primes = [True] * (n + 1)
    primes[0] = False
    primes[1] = False
    for p in range(2, n + 1):
        if primes[p] == True:
            currentMult = p
            currentNum = p * currentMult
            while currentNum < n + 1:
                primes[currentNum] = False
                currentMult += 1
                currentNum = currentMult * p
    return primes

#just add up all of the numbers that are marked true
def calculateSummationOfPrimesLessThenN(n):
    primes = sieveOfEratostehnes(n)
    currentSum = 0
    for index in range(0, len(primes)):
        if primes[index]:
            currentSum += index
    return currentSum


print calculateSummationOfPrimesLessThenN(2000000)