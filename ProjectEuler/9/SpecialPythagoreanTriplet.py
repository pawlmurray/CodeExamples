#finds the c in a^2 + b^2 = c^2
def findCInPythagoreanTheorem(a, b):
    return (a**2 + b**2)**.5

#checks to see if c is a whole integer
def isCWholeInteger(c):
    return float(c).is_integer()

#checks t osee if the pythagorean triplet adds up to n
def pythagoreanTripletAddsUpToN(a, b, c, n):
    return (a + b + c) == n

#finds a pythagorean triplet (a^2 + b^2 = c^2) such that
# a + b + c = n
def findSpecialPythagoreanTripletThatAddsUpToN(n):
    for a in range(1, n/2):
        for b in range(a + 1, n/2 +  1):
            c = findCInPythagoreanTheorem(a, b)
            if isCWholeInteger(c) and pythagoreanTripletAddsUpToN(a, b, int(c), n):
                return a * b * int(c)
    return -1


print findSpecialPythagoreanTripletThatAddsUpToN(1000)