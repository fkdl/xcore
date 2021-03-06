﻿/*==============================================================================
    文件名称：Redis.cs
    适用环境：CoreCLR 5.0,.NET Framework 2.0/4.0/5.0
    功能描述：Redis驱动类
================================================================================
 
    Copyright 2015 XieChaoyi

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

               http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.

===============================================================================*/
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Wlniao.Caching
{
    /// <summary>
    /// InMemory缓存
    /// </summary>
    public class InMemory
    {
        private static Dictionary<String, CacheData> cache = new Dictionary<String, CacheData>();


        /// <summary>
        /// 设置缓存内容
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireSeconds"></param>
        public static Boolean Set(String key, String value, int expireSeconds = 86400)
        {
            if (cache.ContainsKey(key))
            {
                cache[key].Expire = DateTime.Now.AddSeconds(expireSeconds);
                cache[key].Value = value;
            }
            else
            {
                var data = new CacheData();
                data.Expire = DateTime.Now.AddSeconds(expireSeconds);
                data.Value = value;
                cache.Add(key, data);
            }
            return true;
        }


        /// <summary>
        /// 设置缓存内容
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expireSeconds"></param>
        /// <returns></returns>
        public static Boolean Set<T>(String key, T obj, int expireSeconds = 86400)
        {
            if (cache.ContainsKey(key))
            {
                cache[key].Expire = DateTime.Now.AddSeconds(expireSeconds);
                cache[key].Value = obj;
            }
            else
            {
                var data = new CacheData();
                data.Expire = DateTime.Now.AddSeconds(expireSeconds);
                data.Value = obj;
                cache.Add(key, data);
            }
            return true;
        }



        /// <summary>
        /// 删除缓存内容
        /// </summary>
        /// <param name="key"></param>
        public static Boolean Del(String key)
        {
            if (cache.ContainsKey(key))
            {
                return cache.Remove(key);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否存在缓存项
        /// </summary>
        /// <param name="key"></param>
        public static Boolean Exists(String key)
        {
            if (cache.ContainsKey(key))
            {
                if (cache[key].Expire > DateTime.Now)
                {
                    return true;
                }
                else
                {
                    cache.Remove(key);
                }
            }
            return false;
        }



        /// <summary>
        /// 获取一个缓存项
        /// </summary>
        /// <param name="key"></param>
        public static String Get(String key)
        {
            if (cache.ContainsKey(key))
            {
                if (cache[key].Expire > DateTime.Now)
                {
                    return cache[key].Value.ToString();
                }
                else
                {
                    cache.Remove(key);
                }
            }
            return "";
        }
        /// <summary>
        /// 获取一个缓存项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(String key)
        {
            if (cache.ContainsKey(key))
            {
                if (cache[key].Expire > DateTime.Now)
                {
                    return (T)cache[key].Value;
                }
                else
                {
                    cache.Remove(key);
                }
            }
            return default(T);
        }
    }
}
