using System;
using UnityEngine;

public class CustomExceptionTest : MonoBehaviour
{
    private void Start()
    {
        Debug.LogException(new CustomException("This is a custom exception!"));

        try
        {
            DoAThing();
        }
        catch (CustomException exception)
        {
            Debug.LogException(exception);
        }
    }

    private void DoAThing()
    {
        SecretlyThrows();
    }

    private void SecretlyThrows()
    {
        throw new CustomException("Exception throw with callstack");
    }
}

public class CustomException : Exception
{
    public CustomException(string message) : base(message) { }
}
