1. 创建用户的时候报错 ERROR 1396 (HY000): Operation CREATE USER failed for 'user1'@'%'

```
mysql> create user user1 IDENTIFIED by '123456';
ERROR 1396 (HY000): Operation CREATE USER failed for 'user1'@'%'
```

2. 上网查了一下说是没有这个用户之前有创建过，删除了，但是可能没有刷新权限，执行刷新权限， 发现还是创建不了

```
mysql> flush privileges; 
Query OK, 0 rows affected (0.01 sec)
 mysql> 
mysql> 
mysql> create user user1 IDENTIFIED by '123456';
```

3. 再次删除用户，再刷新，再创建就可以了

```
mysql> drop user user1;
Query OK, 0 rows affected (0.02 sec)
 mysql> flush privileges; 
Query OK, 0 rows affected (0.01 sec)
 mysql> create user user1 IDENTIFIED by '123456';
Query OK, 0 rows affected (0.01 sec)
```

完！