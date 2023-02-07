# **1** **索引概述**

**什么是索引？**

 **索引就是 MySQL 中可以高效获取数据的数据结构（有序）。**在数据之外，数据库系统还维护着满足 特定查找算法的数据结构，这些数据结构以某种方式引用（指向）数据， 这样就可以在这些数据结构 上实现高级查找算法，这种数据结构就是索引。

 添加索引原则上来说可以大大的提高数据获取的效率。

**索引的优势：**

 提高数据检索的效率，降低数据库 的 IO 成本。

 通过索引列对数据进行排序，降低 数据排序的成本，降低 CPU 的消 耗。

**索引的劣势：**

 索引也是占用空间的。

 索引大大提高了查询效率，同时却也降低更新表的速度， 如对表进行 INSERT、UPDATE、DELETE 时，效率降低。

# **2** **索引结构**

## **2.1** **概述**

 MySQL 的索引是在存储引擎层实现的，不同的存储引擎有不同的索引结构，索引都有 B+Tree 索引、Hash 索引、R-tree(空间索 引） 、Full-text(全文 索引)。我们常用的为 B+Tree 和 Hash 索引。

## **2.2 B+Tree**

 B+Tree 是 B-Tree 的变种，主要有以下三点区别：

 。所有的数据都会出现在叶子节点。

 。叶子节点形成一个单向链表。

 。非叶子节点仅仅起到索引数据作用，具体的数据都是在叶子节点存放的。

 然而 MySQL 中又对 B+Tree 做了优化， 在原 B+Tree 的基础上，增加一个指向相邻叶子节点 的链表指针，就形成了带有顺序指针的 B+Tree ，提高区间访问的性能，利于排序。

## **2.3 Hash**

 MySQL 中除了支持 B+Tree 索引，还支持一种索引类型 ---Hash 索引。

        哈希索引就是采用一定的 hash 算法，将键值换算成新的 hash 值，映射到对应的槽位上，然后存储在 hash 表中。

 如果两个 (或多个) 键值，映射到一个相同的槽位上，他们就产生了 hash 冲突（也称为 hash 碰撞），可 以通过链表来解决，如果了解 java 的话应该很好理解。

**Hash 索引的优点：**

 查询效率高，通常 (不存在 hash 冲突的情况) 只需要一次检索就可以了，效率通常要高于 B+tree 索引。

**Hash 索引的缺点：**

 Hash 索引只能用于对等比较 (= ， in) ，不支持范围查询（ between ， > ， < ， ... ） 。

         无法利用索引完成排序操作 。

# **3** **索引分类**

## **3.1 索引分类**

 在 MySQL 数据库，将索引的具体类型主要分为以下几类：主键索引、唯一索引、常规索引、全文索引。

![](https://img-blog.csdnimg.cn/43fc21df37a8495ebee32d5c02d73615.png)

## **3.2** **聚集索引** **&** **二级索引**

 而在在 InnoDB 存储引擎（MySQL 默认存储引擎）中，根据索引的存储形式，又可以分为以下两种：

![](https://img-blog.csdnimg.cn/83dfb55dcb024125aebfd10e4653b04b.png)

###  聚集索引选取规则:

 如果存在主键，主键索引就是聚集索引。

 如果不存在主键，将使用第一个唯一（UNIQUE）索引作为聚集索引。

 如果表没有主键，且没有合适唯一索引，InnoDB 会自动生成一个 rowid 作为隐藏的聚集索引。

**聚集索引** **&** **二级索引的区别：**

 聚集索引的叶子节点下挂的是这一行的数据 。

 二级索引的叶子节点下挂的是该字段值对应的主键值。

# **4** **索引语法**

**创建索引：**

 CREATE [UNIQUE | FULLTEXT] INDEX index_name ON table_name (index_col_name,...) ;

 **演示**：例如给 user 表的 name 字段添加索引，中括号里的参数是可以省略的。

 CREATE INDEX idx_user_name ON user(name);

 **查看索引：**

 SHOW INDEX FROM table_name ;

 **演示**：例如查看 user 表中的索引。

 SHOW INDEX FROM user ;

  
**删除索引：**

 DROP INDEX index_name ON table_name ;

 **演示**：例如删除我们刚刚创建的索引。

 DROP INDEX idx_user_name ON user ;

# **5** **索引使用**

 介绍索引的使用之前，我们先要了解一下 explain 这个函数

## **explain**

 EXPLAIN 或者 DESC 命令获取 MySQL 如何执行 SELECT 语句的信息，包括在 SELECT 语句执行 过程中表如何连接和连接的顺序。

**语法：**

        直接在 select 语句之前加上关键字 explain / desc 即可。

 EXPLAIN SELECT 字段列表 FROM 表名 WHERE 条件 ;

 **例如**：我要查询 user 表中 id=1 的数据，其中 id 为主键索引

 explain select * from tb_user where id = 1;

![](https://img-blog.csdnimg.cn/469eefa1d8a846aba1cba12a1ffdc20e.png)

 简单介绍一下比较重要的参数：

**type：**表示连接类型，性能由好到差的连接类型为 NULL、system、const、 eq_ref、ref、range、 index、all 。在我们编写代码时，type 尽量要往前靠。

**possible_key：** 显示可能应用在这张表上的索引，一个或多个。

**key：** 实际使用的索引，如果为 NULL ，则没有使用索引。

**key_len：** 表示索引中使用的字节数， 该值为索引字段最大可能长度，并非实际使用长度，在不损失精确性的前提下， 长度越短越好 。

## 5.1 最左前缀法则

 如果索引了多列（联合索引），要遵守最左前缀法则。最左前缀法则指的是查询从索引的最左列开始，并且不跳过索引中的列。如果跳跃某一列，索引将会部分失效 (后面的字段索引失效)。

 以 tb_user 表为例，我们先看一下 tb_user 表创建的索引。

![](https://img-blog.csdnimg.cn/9beb5f0fac5d453bbbc9503f3abc4da4.png)

 在 tb_user 表中，有一个联合索引，这个联合索引涉及到三个字段，顺序分别为：profession， age，status。

在执行以下代码时：

 explain select * from tb_user where profession = '软件工程' and age = 31 and status = '0';

![](https://img-blog.csdnimg.cn/f22977107da24e60870a27cb4205e7ee.png)

         可以看到在这里使用到了索引的，key_len 为 54。

        explain select * from tb_user where profession = '软件工程';

![](https://img-blog.csdnimg.cn/1d2eb9c36df54872810e88fc2b203d49.png)

        可以看到满足最左前缀法则时是可以用到索引的，key_len=47;

        explain select * from tb_user where profession = '软件工程' and status = '0';

![](https://img-blog.csdnimg.cn/5d3e15f77a604156bf9f33406c156566.png)

         可以看到 status 字段是不符合最左前缀法则的，跳过了联合索引中间的 age 字段，这时的 key_len 的 长度也为 47。所以可以得出若不满足最最前缀法则实惠导致联合索引部分失效的情况。

## **5.2 范围查询**

 联合索引中，出现范围查询 (>,<)，范围查询右侧的列索引失效。

 explain select * from tb_user where profession = '软件工程'and age > 30 and status ='0';

![](https://img-blog.csdnimg.cn/7e8336e70ed8446ca192957f40034382.png)

  当范围查询使用 > 或 < 时，走联合索引了，联合索引全走长度为 54 这里索引的长度为 49，就说明范围查询右边的 status 字段是没有走索引的。

## 5.3 索引失效情况

### **5.3.1 字符串不加引号**

        字符串类型字段使用时，不加引号，索引将失效

 explain select * from tb_user where profession = '软件工程' and age = 31 and status = 0;

![](https://img-blog.csdnimg.cn/ab74698ef212469a9816ec254c27f37b.png)

         如果字符串不加单引号，对于查询结果，没什么影响，但是数据库存在隐式类型转换，key_len 长度为 49，索引部分失效了。

### 5.3.2 **索引列运算**

 若在索引列上进行运算操作， 索引则会失效。

 explain select * from tb_user where substring(profession,1,2) = '软件' and age = 31 and status = 0;

![](https://img-blog.csdnimg.cn/fae05aee114444fb872dfeb34f5f6172.png)

  
        可以看到联合索引直接全部失效了，这里若是单例索引也会失效的。

### 5.3.3 **模糊查询**

        如果仅仅是尾部模糊匹配，索引不会失效。如果是头部模糊匹配，索引失效。

执行以下两句代码：

 explain select * from tb_user where profession like '软件 %';

 explain select * from tb_user where profession like '% 工程';

![](https://img-blog.csdnimg.cn/8c7889999eb54334b06d663208bc7198.png)

  
        经过上述的测试，我们发现，在 like 模糊查询中，在关键字后面加 %，索引可以生效。而如果在关键字 前面加了 %，索引将会失效。

### 5.3.4 **or** **连接条件**

 用 or 分割开的条件， 如果 or 前的条件中的列有索引，而后面的列中没有索引，那么涉及的索引都不会被用到。

 先看下 tb_user 表的索引情况。

![](https://img-blog.csdnimg.cn/3ffa6acbbb62436ab2579d605de15716.png)

        explain select * from tb_user where id = 10 or age = 23;

![](https://img-blog.csdnimg.cn/0f321fbf269c433dbf38d77065bd1a4b.png)

         可以看到 id 是主键索引，而 age 并没有索引，所以这里索引失效了。

## 5.4 SQL 提示

 SQL 提示，是优化数据库的一个重要手段，简单来说，就是在 SQL 语句中加入一些人为的提示来达到优 化操作的目的。

        use index ： 建议 MySQL 使用哪一个索引完成此次查询（仅仅是建议，mysql 内部还会再次进 行评估）。

 explain select * from tb_user use index(idx_user_pro) where profession = '软件工程';

![](https://img-blog.csdnimg.cn/38c3dbbda27d4eefbffda3fd653a9213.png)

        ignore index ： 忽略指定的索引。

 explain select * from tb_user ignore index(idx_user_pro) where profession = '软件工程';

![](https://img-blog.csdnimg.cn/504c11bbeabd453a9f87f1a1aa2df80b.png)

        force index ： 强制使用索引。

 explain select * from tb_user force index(idx_user_pro) where profession = '软件工程';

![](https://img-blog.csdnimg.cn/7c6114ca3bc744a589e616fe1029bb1b.png)

## 5.5 覆盖索引

        尽量使用覆盖索引，减少 select *。 那么什么是覆盖索引呢？ 覆盖索引是指 查询使用了索引，并且需要返回的列，在该索引中已经全部能够找到 。

 也就是你要获取的数据就在叶子节点存放的键值以及主键 id 内。

## 5.6 索引前缀

        当字段类型为字符串（varchar，text，longtext 等）时，有时候需要索引很长的字符串，这会让索引变得很大，查询时，浪费大量的磁盘 IO， 影响查询效率。此时可以只将字符串的一部分前缀，建 立索引，这样可以大大节约索引空间，从而提高索引效率。

**语法：** create index idx_xxxx on table_name(column(n)) ;

**示例****:** 为 tb_user 表的 email 字段，建立长度为 5 的前缀索引。

 create index idx_email_5 on tb_user(email(5));

![](https://img-blog.csdnimg.cn/340eb483f23242d0a4f4b4647ddb8b7e.png)

## 5.7 单例索引和联合索引 

        单列索引：即一个索引只包含单个列。

 联合索引：即一个索引包含了多个列。

 在业务场景中，如果存在多个查询条件，考虑针对于查询字段建立索引时，建议建立联合索引， 而非单列索引。

# 6 索引设置原则

1). 针对于数据量较大，且查询比较频繁的表建立索引。

2). 针对于常作为查询条件（where）、排序（order by）、分组（group by）操作的字段建立索 引。

3). 尽量选择区分度高的列作为索引，尽量建立唯一索引，区分度越高，使用索引的效率越高。

4). 如果是字符串类型的字段，字段的长度较长，可以针对于字段的特点，建立前缀索引。

5). 尽量使用联合索引，减少单列索引，查询时，联合索引很多时候可以覆盖索引，节省存储空间，避免回表，提高查询效率。

6). 要控制索引的数量，索引并不是多多益善，索引越多，维护索引结构的代价也就越大，会影响增删改的效率。

**码字不易，可以点个免费的赞！！**