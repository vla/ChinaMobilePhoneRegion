# ChinaMobilePhoneRegion

[![Build Status](https://travis-ci.org/vla/ChinaMobilePhoneRegion.svg?branch=master)](https://travis-ci.org/vla/ChinaMobilePhoneRegion)


高性能中国手机号码归属地查询库，内置43万个号码段，文件压缩后只有1MB。

提供内存及IO加载方式查询，建议装载到内存上查询性能更高。

Packages & Status
---

Package  | NuGet         |
-------- | :------------ |
|**ChinaMobilePhoneRegion**|[![NuGet package](https://buildstats.info/nuget/ChinaMobilePhoneRegion)](https://www.nuget.org/packages/ChinaMobilePhoneRegion)

Performance
---

I7-6700K 32G DDR4

单条查询`0.0042ms`，以下为`20 million`调用性能：


 Kind    | Total Time   | Per Second |
---------|--------------|:-----------|
1 thread | 7815.7194ms  | 250m+
4 threads| 2356.6162ms  | 840m+
parallel | 2111.2532ms  | 940m+

Usage
---

建议采取内置数据源方式使用：

```cs

var searcher = MobilePhoneFactory.GetSearcher();

if(searcher.TryGet("13702331111", out MobilePhone info))
{
    Console.WriteLine(info);
}

OR

//查询结果
var result = searcher.Search(1370233);

if(result.Success)
{
    //成功后使用行政编码获取相应信息
    var adCode = ChinaAdCode.Get(result.AdCode);
}

```

> 数据源或者`ISearcher`对象建议定义为全局单例来使用

内存数据源：

```cs
//将数据加载到内存中进行搜索，服务器级别的硬件能达到千万级每秒查询
var dataSource = new MemoryDataSource(filename);

var searcher = MobilePhoneFactory.GetSearcher(dataSource);

if(searcher.TryGet("13702331111", out MobilePhone info))
{
    Console.WriteLine(info);
}

dataSource.Dispose();
```

IO数据源：

```cs

//使用IO方式进行搜索
var dataSource = new StreamDataSource(filename);

var searcher = MobilePhoneFactory.GetSearcher(dataSource);

if(searcher.TryGet("13702331111", out MobilePhone info))
{
    Console.WriteLine(info);
}

dataSource.Dispose();

```

手机归属存储的数据内容是行政区域编码，其中内置了中国行政区域信息查询库，完整使用请查看测试用例：

```cs

var info = ChinaAdCode.Get(440402);

var info = ChinaAdCode.Search("广东省");

```