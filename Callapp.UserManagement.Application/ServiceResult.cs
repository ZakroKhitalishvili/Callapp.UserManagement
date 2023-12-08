using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Callapp.UserManagement.Application;

public class ServiceResult
{
    public bool Successful { get; init; }
    public ICollection<string> Errors { get; init; } = new List<string>();

    public static ServiceResult Success() => new() { Successful = true };

    public static ServiceResult Error(string error) => new() { Successful = false, Errors = [error] };

    public static ServiceResult Error(ICollection<string> errors) => new() { Successful = false, Errors = errors };
}


public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; init; }

    public static ServiceResult<T> Success(T data) => new() { Successful = true, Data = data };

    public new static ServiceResult<T> Error(string error) => new() { Successful = false, Errors = [error] };

    public new static ServiceResult<T> Error(ICollection<string> errors) => new() { Successful = false, Errors = errors };
}