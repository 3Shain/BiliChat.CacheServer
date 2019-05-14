# Bilichat 后端头像缓存/统计服务

[前端项目地址](https://github.com/3Shain/Bilichat)

## 需要设置的参数

`appsetting.json` 的`ConnectionStrings`中需要设置mysql连接字符串和redis服务器地址
数据库需要运行migration迁移命令！在VisualStudio的程序包管理控制台输入`Add-Migration initial` `Update-Database`