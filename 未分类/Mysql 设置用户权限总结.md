**1. 添加用户：**

**快速创建用户：**

```
格式：   create user 'username'@'ip地址' identified by '密码'
```

**（ 注意：ip 地址所选范围： % 为所有 ip   localhost 只为本地连接 ）**

**一般创建用户并赋予特定权限：**

```
格式：     grant 权限 on 数据库.* to 用户名@登录的主机 identified by "密码";        【 默认全部操作权限 】
```

**例 1：增加一个 admin1 用户，密码为 123，可以在任何主机上登录，并对所有数据库有查询、增加、修改和删除的功能。需要在 mysql 的 root 用户下进行**

```
例如：     grant select,insert,update,delete on *.* to admin1@"%" identified by "123";
       flush privileges;//刷新数据库
```

**例 2：增加一个 admin2 用户，密码为 123，只能在 101.200.100.42** **上登录，并对数据库 student 有查询，增加，修改和删除的功能。需要在 mysql 的 root 用户下进行**

```
例如：     grant select,insert,update,delete on *.* to admin1@101.200.100.42 identified by "123";
       flush privileges;//刷新数据库
```

添加用户可能会出现密码验证强度过高的提示，解决方法为：

```
SHOW VARIABLES LIKE 'validate_password%'; 进行查看密码强度
```

```
set global validate_password_policy=LOW; ” 进行设值密码强度为低
```

**2. 更新用户名或者 ip：**

```
rename user '旧的用户名'@'旧的ip地址'  to '新的用户名'@'新的ip地址'
```

**3. 登录用户：**

```
mysql  -u用户名 -pIP地址 -p     
```

**（** **注意：创建指定 ip 地址的用户，登录时需要指定对应的 ip 地址** **）**

**4.** **授权用户 admin3 拥有数据库 student 的所有权限**

```
grant all privileges on student.* to admin3@localhost identified by ’123′;
```

```
flush privileges;
```

**5. 修改用户密码**

```
update mysql.user set password=password(’123456′) where User=’admin1′ and Host=’localhost’;
```

```
flush privileges;
```

**6. 删除用户**

```
drop user 用户名@'%'
drop user 用户名@localhost
```

```
delete from user where user=”用户名”
```

**7.grant 普通数据用户，查询、插入、更新、删除数据库中所有表数据的权利**

```
grant select on admindb.* to common_user@’%’
grant insert on admindb.* to common_user@’%’
grant update on admindb.* to common_user@’%’
grant delete on admindb.* to common_user@’%’
```

或者一条查询所有：

```
grant select, insert, update, delete on admindb.* to common_user@’%’
```

**8.grant 数据库开发人员，创建表、索引、视图、存储过程、函数。。。等权限**

```
grant 创建、修改、删除 MySQL 数据表结构权限。
grant create on testdb.* to developer@’192.168.0.%’;
grant alter on testdb.* to developer@’192.168.0.%’;
grant drop on testdb.* to developer@’192.168.0.%’;
grant 操作 MySQL 外键权限。
grant references on testdb.* to developer@’192.168.0.%’;
grant 操作 MySQL 临时表权限。
grant create temporary tables on testdb.* to developer@’192.168.0.%’;
grant 操作 MySQL 索引权限。
grant index on testdb.* to developer@’192.168.0.%’;
grant 操作 MySQL 视图、查看视图源代码 权限。
grant create view on testdb.* to developer@’192.168.0.%’;
grant show view on testdb.* to developer@’192.168.0.%’;
grant 操作 MySQL 存储过程、函数 权限。
grant create routine on testdb.* to developer@’192.168.0.%’; — now, can show procedure status
grant alter routine on testdb.* to developer@’192.168.0.%’; — now, you can drop a procedure
grant execute on testdb.* to developer@’192.168.0.%’;
```

**9.grant 普通 test1 管理某个 MySQL 数据库的权限**

```
grant all privileges on admindb to test1@'localhost'
其中，关键字 “privileges” 可以省略。
```

**  
10.grant 高级 test1 管理 MySQL 中所有数据库的权限**

```
grant all on *.* to test1@'localhost'
```

**11.grant 作用在整个服务器上：**

```
grant select on *.* to admin1@localhost; — admin1 可以查询 MySQL 中所有数据库中的表
grant all    on *.* to admin1@localhost; — admin1 可以管理 MySQL 中的所有数据库
```

**12.grant 作用在单个数据库上：**

```
grant select on admindb.* to admin@localhost;  — admin 可以查询 admindb中的表
```

```
grant select,insert,update,delete on admindb.orders to admin@localhost;  — admin 可以查询、添加、修改及删除 admindb中的表orders
```

 **13.grant 作用在表中的列上：**

```
grant select(id, name, age) on admindb.goods_log to admin@localhost;
```

**14.grant 作用在存储过程、函数上：**

```
grant execute on procedure admindb.goods_log to admin@localhost
grant execute on function admindb.goods_log to admin@localhost
```

**15. 查看当前用户（自己）权限：**

```
show grants;
```

**16. 查看其他 MySQL 用户权限：**

```
show grants for admin@localhost;
```

**17. 撤销已经赋予给 MySQL 用户权限的权限;**

```
revoke 跟 grant 的语法差不多，只需要把关键字 “to” 换成 “from” 即可：
grant all on *.* to admin@localhost;
revoke all on *.* from admin@localhost;
```