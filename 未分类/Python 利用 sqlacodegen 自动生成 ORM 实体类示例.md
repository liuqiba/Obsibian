> 本文由 [简悦 SimpRead](http://ksria.com/simpread/) 转码， 原文地址 [pythonjishu.com](https://pythonjishu.com/kgcrdzjuprpzaey/)

> 当我们使用 Python 进行数据库操作时，可以使用 ORM（对象关系映射）来帮助我们简化 SQL 操作，将数据库表的记录映射成 Python 对象进行操作，ORM 工具中最流行的就是 SQLAlchemy 库。

2023 年 6 月 3 日 上午 12:32 • [python](https://pythonjishu.com/python/python-2/)

当我们使用 Python 进行数据库操作时，可以使用 ORM（对象关系映射）来帮助我们简化 SQL 操作，将数据库表的记录映射成 Python 对象进行操作，ORM 工具中最流行的就是 SQLAlchemy 库。

但是，在使用 SQLAlchemy 库时，我们需要手动编写 ORM 实体类，这样会占用很多时间和精力。因此，我们可以使用 sqlacodegen 工具自动生成 ORM 实体类。以下是详细步骤：

前提条件
----

首先，我们需要安装以下软件：

*   Python 3 环境
*   SQL 数据库（如 MySQL 或 PostgreSQL）
*   SQLAlchemy 库
*   sqlacodegen 库

可以使用以下命令安装 SQLAlchemy 和 sqlacodegen 库：

```
pip install SQLAlchemy
pip install sqlacodegen


```

生成 ORM 实体类
----------

假设我们有一个名叫 `test` 的数据库，其中包含一个名为 `users` 的表。我们可以使用以下命令生成对应的 ORM 实体类：

```
sqlacodegen --outfile=models.py mysql://username:password@localhost/test


```

其中，`--outfile` 参数指定生成的 ORM 实体类所在的文件名，`mysql://username:password@localhost/test` 是数据库连接字符串，这里需要根据实际情况进行修改。

生成的 `models.py` 文件内容如下：

```
# coding: utf-8
from sqlalchemy import BIGINT, Column, DateTime, Integer, String, text
from sqlalchemy.ext.declarative import declarative_base


Base = declarative_base()
metadata = Base.metadata


class User(Base):
    __tablename__ = 'users'

    id = Column(BIGINT(20), primary_key=True)
    name = Column(String(255))
    age = Column(Integer)
    created_time = Column(DateTime, server_default=text("CURRENT_TIMESTAMP"))


```

我们可以使用生成的 `User` 类来操作 `users` 表中的数据。例如，查询 `users` 表中的所有数据：

```
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from models import User


engine = create_engine('mysql://username:password@localhost/test')
Session = sessionmaker(bind=engine)
session = Session()

users = session.query(User).all()
for user in users:
    print(user.name)


```

注意事项
----

在生成 ORM 实体类时，有一些注意事项需要注意：

*   如果数据库中表的字段名出现 Python 的关键字，sqlacodegen 会在生成 ORM 实体类时给该字段添加下划线前缀，例如 `return` 字段会被转化为 `_return`。
*   sqlacodegen 会根据数据库中表的字段类型自动推断 ORM 实体类中字段的类型，但是有些类型可能会被推断错误，需要我们手动修改生成的代码。
*   如果数据库中某个表的主键不是自动增长（auto_increment），则需要在生成的 ORM 实体类中手动设置主键的值。

示例说明
----

以下是两个示例说明，展示如何使用 sqlacodegen 自动生成 ORM 实体类。

### 示例 1

假设我们有一个名为 `test` 的 MySQL 数据库，其中有一个名为 `books` 的表，其中每个书籍包含 `id`、`name` 和 `author` 三个字段。现在我们需要使用 sqlacodegen 自动生成对应的 ORM 实体类代码：

```
sqlacodegen --outfile=models.py mysql://username:password@localhost/test


```

这里的 `--outfile` 参数指定了生成的 ORM 实体类代码存放的文件名，可以根据实际情况进行修改。生成的 `models.py` 文件内容如下：

```
# coding: utf-8
from sqlalchemy import Column, Integer, String
from sqlalchemy.ext.declarative import declarative_base


Base = declarative_base()
metadata = Base.metadata


class Book(Base):
    __tablename__ = 'books'

    id = Column(Integer, primary_key=True)
    name = Column(String(255), nullable=False)
    author = Column(String(255), nullable=False)


```

生成的 `models.py` 文件中包含了一个名为 `Book` 的 ORM 实体类，我们可以使用该类来操作 `books` 表中的数据。

### 示例 2

假设我们有一个名为 `test` 的 PostgreSQL 数据库，其中有一个名为 `products` 的表，其中每个商品包含 `id`、`name`、`price` 和 `stock` 四个字段。其中，`id` 字段为主键，但不是自动增长的。现在我们需要使用 sqlacodegen 自动生成对应的 ORM 实体类代码：

```
sqlacodegen --outfile=models.py postgresql://username:password@localhost/test


```

这里的 `--outfile` 参数指定了生成的 ORM 实体类代码存放的文件名，可以根据实际情况进行修改。生成的 `models.py` 文件内容如下：

```
# coding: utf-8
from sqlalchemy import Column, Integer, String
from sqlalchemy.ext.declarative import declarative_base


Base = declarative_base()
metadata = Base.metadata


class Product(Base):
    __tablename__ = 'products'

    id = Column(Integer, primary_key=True)
    name = Column(String(255), nullable=False)
    price = Column(Integer, nullable=False)
    stock = Column(Integer, nullable=False)


```

生成的 `models.py` 文件中包含了一个名为 `Product` 的 ORM 实体类，我们可以使用该类来操作 `products` 表中的数据。注意，在该表中，主键不是自动增长的，因此我们需要手动为 `id` 字段设置值。以下是一个示例代码：

```
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker
from models import Product


engine = create_engine('postgresql://username:password@localhost/test')
Session = sessionmaker(bind=engine)
session = Session()

product = Product()
product.id = 1
product.name = 'Apple'
product.price = 10
product.stock = 100

session.add(product)
session.commit()


```