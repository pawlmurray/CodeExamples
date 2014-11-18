def fibonacchiEvenSum(max):
    prev = 1
    currentSum = 0
    currentValue = 2
    while currentValue < max:
        if currentValue % 2 == 0:
            currentSum += currentValue
        oldValue = currentValue
        currentValue += prev
        prev = oldValue
    return currentSum

print fibonacchiEvenSum(4000000)