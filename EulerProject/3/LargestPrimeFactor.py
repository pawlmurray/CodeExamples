#probably could've made this faster
def findBiggestPrimeFactor(num):
	primeFactors = []
	divider = 2
	while num > 1:
		while num % divider == 0:
			num /= divider
			primeFactors.append(divider)
		divider += 1
	return primeFactors[-1]


print findBiggestPrimeFactor(600851475143)