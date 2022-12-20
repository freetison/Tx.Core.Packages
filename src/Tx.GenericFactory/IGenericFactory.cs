using System;
using System.Collections.Generic;

namespace Tx.Core.GenericFactory;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <typeparam name="TK"></typeparam>
public interface IGenericFactory<in T, TK> where TK : class
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="ctor"></param>
    void Register(T type, Func<TK> ctor);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    TK Get(T type);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IList<Func<TK>> GetAll();
}