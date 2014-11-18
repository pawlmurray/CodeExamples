#returns true if the number is a palindrome
def isPalindrome(num):
    numAsStr = str(num)
    for index in range (0, len(numAsStr) / 2):
        if not numAsStr[index] == numAsStr[-(index+1)]:
            return False
    return True


#given one number, find the largest palindrome that number can make with another triple digit number
def findLargestPalindromeAndOtherFactor(num1):
    num2 = 999
    hasFoundPalindrome = False
    while num2 > 99:
        if isPalindrome(num1 * num2):
            return num1 * num2
        num2-=1
    return -1

#finds the largest triple digit palindrome
def findLargestTripleDigitPalindrome():
    numToTry = 999
    biggestPalindrome = -1
    while numToTry > 99:
        palindromeAttempt = findLargestPalindromeAndOtherFactor(numToTry)
        if not palindromeAttempt == -1 and palindromeAttempt > biggestPalindrome:
            biggestPalindrome = palindromeAttempt
        numToTry-=1
    return biggestPalindrome  


print findLargestTripleDigitPalindrome()