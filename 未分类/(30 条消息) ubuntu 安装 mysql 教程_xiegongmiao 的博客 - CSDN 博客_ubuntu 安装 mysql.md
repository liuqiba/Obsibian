                                                                                [ubuntu](https://so.csdn.net/so/search?q=ubuntu&spm=1001.2101.3001.7020) 安装 mysql 教程

一. 首先卸载掉原来的 mysql

第一步，依次执行下面的语句

sudo apt-get autoremove --purge mysql-server  
sudo apt-get remove mysql-server  
sudo apt-get autoremove mysql-server  
sudo apt-get remove mysql-common 

第 2 步 清理残留数据

dpkg -l |grep ^rc|awk '{print $2}' |sudo xargs dpkg -P

二. 安装 mysql

安装 mysql 教程  
sudo aptitude search  mysql  
sudo apt install  mysql-server

登录 mysql

sudo mysql -u root -p  
show databases;

use mysql;

show tables;  
select host,user from user;  
#添加登录账号, 允许远程访问'%'  
grant all privileges on *.* to 'admin'@'%' identified by 'pass12345' with grant option;

注意：如果报 ERROR 1064 (42000): You have an error in your SQL syntax; check the manual that corresponds to your MySQL server version for the right syntax to use near '. to'admin'@'%'' at line 1

提示意思是不能用 grant 创建用户，mysql8.0 以前的版本可以使用 grant 在授权的时候隐式的创建用户，8.0 以后已经不支持，所以必须先创建用户，然后再授权，命令如下：

 CREATE USER 'admin'@'%' IDENTIFIED BY 'pass12345';

 grant all privileges on *.* to 'admin'@'%';

**允许多个 IP 登录 mysql8.0**

```
# 第一个IP
 
CREATE user 'admin'@'121.43.128.209' IDENTIFIED by 'pass12345';
GRANT ALL on *.* TO 'admin'@'121.43.128.209';
flush privileges;
 
# 第二个IP
 
CREATE user 'admin'@'139.224.43.133' IDENTIFIED by 'pass12345';
GRANT ALL on *.* TO 'admin'@'139.224.43.133';
flush privileges;
 
# 有几个IP，就创建多少个用户
```

修改允许的 ip

UPDATE `user` SET `Host`='172.0.0.1' WHERE `user`='admin' AND `Host`='175.9.142.131';  
flush privileges;

![](https://img-blog.csdnimg.cn/c12636288376497282e30d66a671f050.png)

如果要删除某个 IP 权限

```
delete from user where host ='172.0.0.2';
```

  
#刷新  
flush privileges;  
quit;  
#重启  
sudo service mysql restart;  
#查看 mysql 版本  
mysql -V

第三步  设置 MySQL 远程访问的权限   
aptitude search  mysql.conf.d  
cd /etc/mysql/mysql.conf.d  
ls  
#查看目录下所有的文件内容，找到 bind-address = 127.0.0.1 注释掉  
sudo vim mysqld.cnf  
加前缀 #注释掉 bind-address = 127.0.0.1

![](https://img-blog.csdnimg.cn/20200420164959420.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3hpZWdvbmdtaWFv,size_16,color_FFFFFF,t_70)

![](https://img-blog.csdnimg.cn/2020042016540360.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L3hpZWdvbmdtaWFv,size_16,color_FFFFFF,t_70)

  
重启 mysql  
sudo service mysql restart;

最后重新登录 mysql

mysql -u root -p

另外：

修改密码  
update mysql.user set password=password('新密码') where User="amdin" ;

mysql 查看端口号  
show variables like 'port';

删除登录用户

```
DROP USER IF EXISTS admin;
```