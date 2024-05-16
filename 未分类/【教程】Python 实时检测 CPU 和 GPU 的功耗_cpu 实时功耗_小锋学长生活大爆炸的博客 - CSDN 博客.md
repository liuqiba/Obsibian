**目录**

[前言](#t0)

[GPU 功耗检测方法](#t1)

[CPU 功耗检测方法](#t2)

[sudo 的困扰与解决](#t3)

[完整功耗分析示例代码](#t4)

转载请注明出处：小锋学长生活大爆炸 [xfxuezhang.cn]

# 前言

        相关一些检测工具挺多的，比如 **powertop、powerstat、s-tui** 等。但如何通过**代码的方式**来实时检测，是个麻烦的问题。通过许久的搜索和自己的摸索，发现了可以**检测 CPU 和 GPU 功耗**的方法。如果有什么不对，或有更好的方法，欢迎评论留言！

        文末附**完整功耗分析的示例代码**！

# GPU 功耗检测方法

        如果是常规的工具，可以使用官方的 NVML。但这里需要 Python 控制，所以使用了对应的封装：pynvml。

        先安装：

```
import pynvml
pynvml.nvmlInit()
 
handle = pynvml.nvmlDeviceGetHandleByIndex(0)
powerusage = pynvml.nvmlDeviceGetPowerUsage(handle) / 1000
```

     关于这个库，网上的使用教程挺多的。这里直接给出简单的示例代码：

```
def get_sensor_values():
    """
    get Sensor values
    :return:
    """
    values = list()
    # get gpu driver version
    version = pynvml.nvmlSystemGetDriverVersion()
    values.append("GPU_device_driver_version:" + version.decode())
    gpucount = pynvml.nvmlDeviceGetCount()  # 显示有几块GPU
    for gpu_id in range(gpucount):
        handle = pynvml.nvmlDeviceGetHandleByIndex(gpu_id)
        name = pynvml.nvmlDeviceGetName(handle).decode()
        meminfo = pynvml.nvmlDeviceGetMemoryInfo(handle)
        # print(meminfo.total)  # 显卡总的显存大小
        gpu_id = str(gpu_id)
        values.append("GPU " + gpu_id + " " + name + " 总共显存大小:" + str(common.bytes2human(meminfo.total)))
        # print(meminfo.used)  # 显存使用大小
        values.append("GPU " + gpu_id + " " + name + " 显存使用大小:" + str(common.bytes2human(meminfo.used)))
        # print(meminfo.free)  # 显卡剩余显存大小
        values.append("GPU " + gpu_id + " " + name + " 剩余显存大小:" + str(common.bytes2human(meminfo.free)))
        values.append("GPU " + gpu_id + " " + name + " 剩余显存比例:" + str(int((meminfo.free / meminfo.total) * 100)))
 
        utilization = pynvml.nvmlDeviceGetUtilizationRates(handle)
        # print(utilization.gpu)  # gpu利用率
        values.append("GPU " + gpu_id + " " + name + " GPU利用率:" + str(utilization.gpu))
 
        powerusage = pynvml.nvmlDeviceGetPowerUsage(handle)
        # print(powerusage / 1000) # 当前功耗, 原始单位是mWa
        values.append("GPU " + gpu_id + " " + name + " 当前功耗(W):" + str(powerusage / 1000))
 
        # 当前gpu power capacity
        # pynvml.nvmlDeviceGetEnforcedPowerLimit(handle)
 
        # 通过以下方法可以获取到gpu的温度，暂时采用ipmi sdr获取gpu的温度，此处暂不处理
        # temp = pynvml.nvmlDeviceGetTemperature(handle,0)
    print('\n'.join(values))
    return values
```

        这个方法获取的值，跟使用 “[nvidia-smi](https://so.csdn.net/so/search?q=nvidia-smi&spm=1001.2101.3001.7020)” 指令得到的是一样的。

![](https://img-blog.csdnimg.cn/e66c82a0eac048bd9e028cc7053823c9.png)

![](https://img-blog.csdnimg.cn/29b89b9c86e9463db6d9a7e8bcbce1ed.png)

         附赠一个来自网上的获取更详细信息的函数：

```
sudo apt install s-tui
pip install s-tui
```

# CPU 功耗检测方法

        这个没有找到开源可以直接用的库。但经过搜索，发现大家都在用的 **s-tui** 工具是开源的！通过查看源码，发现他是有获取 CPU 功耗部分的代码，所以就参考他的源码写了一下。

        先安装：

```
from s_tui.sources.rapl_power_source import RaplPowerSource
 
source.update()
summary = dict(source.get_sensors_summary())
 
cpu_power_total = str(sum(list(map(float, [summary[key] for key in summary.keys() if key.startswith('package')]))))
```

        先直接运行工具看一下效果 (不使用 sudo 是不会出来 Power 的)：

```
from s_tui.sources.rapl_power_source import RaplPowerSource
import socket
import json
 
def output_to_terminal(source):
    results = {}
    if source.get_is_available():
        source.update()
        source_name = source.get_source_name()
        results[source_name] = source.get_sensors_summary()
    for key, value in results.items():
        print(str(key) + ": ")
        for skey, svalue in value.items():
            print(str(skey) + ": " + str(svalue) + ", ")
 
 
source = RaplPowerSource()
# output_to_terminal(source)
 
s = socket.socket()
host = socket.gethostname()
port = 8888
s.bind((host, port))
s.listen(5)
print("等待客户端连接...")
while True:
    c, addr = s.accept()
    source.update()
    summary = dict(source.get_sensors_summary())
    #msg = json.dumps(summary)
    # package表示CPU，dram表示内存(一般不准)
    power_total = str(sum(list(map(float, [summary[key] for key in summary.keys() if key.startswith('package')]))))
    print(f'发送给{addr}：{power_total}')
    c.send(power_total.encode('utf-8'))
    c.close()                # 关闭连接 
```

![](https://img-blog.csdnimg.cn/ca0fefbcb3ed4e4199ebc2f78f27fc72.png)

        说明这个工具确实能获取到 CPU 的功耗。其中 package 就是 2 个 CPU，[dram](https://so.csdn.net/so/search?q=dram&spm=1001.2101.3001.7020) 是内存条功耗 (一般不准，可以不用)。

        直接给出简单的示例代码：

```
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
host = socket.gethostname()
port = 8888
s.connect((host, port))
msg = s.recv(1024)
s.close()
power_usage_cpu = float(msg.decode('utf-8'))
```

        不过注意！由于需要 sudo 权限，所以运行这个 py 文件时候，也需要 sudo 方式，比如：

```
import cv2
import socket
import sys
import threading
import json
import statistics
from psutil import _common as common
import pynvml
pynvml.nvmlInit()
 
class Timer: 
    def __init__(self, name = '', is_verbose = False):
        self._name = name 
        self._is_verbose = is_verbose
        self._is_paused = False 
        self._start_time = None 
        self._accumulated = 0 
        self._elapsed = 0         
        self.start()
 
    def start(self):
        self._accumulated = 0         
        self._start_time = cv2.getTickCount()
 
    def pause(self): 
        now_time = cv2.getTickCount()
        self._accumulated += (now_time - self._start_time)/cv2.getTickFrequency() 
        self._is_paused = True   
 
    def resume(self): 
        if self._is_paused: # considered only if paused 
            self._start_time = cv2.getTickCount()
            self._is_paused = False                      
 
    def elapsed(self):
        if self._is_paused:
            self._elapsed = self._accumulated
        else:
            now = cv2.getTickCount()
            self._elapsed = self._accumulated + (now - self._start_time)/cv2.getTickFrequency()        
        if self._is_verbose is True:      
            name =  self._name
            if self._is_paused:
                name += ' [paused]'
            message = 'Timer::' + name + ' - elapsed: ' + str(self._elapsed) 
            timer_print(message)
        return self._elapsed   
 
class PowerUsage:
    '''
    demo:
        power_usage = PowerUsage()
        power_usage.analyze_start()
        time.sleep(2)
        time_used, power_usage_gpu, power_usage_cpu = power_usage.analyze_end()
        print(time_used)
        print(power_usage_gpu)
        print(power_usage_cpu)
    '''
    def __init__(self):
        self.start_analyze = False
        self.power_usage_gpu_values = list()
        self.power_usage_cpu_values = list()
        self.thread = None
        self.timer = Timer(name='GpuPowerUsage', is_verbose=False)
 
    def analyze_start(self, gpu_id=0, delay=0.1):
        handle = pynvml.nvmlDeviceGetHandleByIndex(gpu_id)
        def start():
            self.power_usage_gpu_values.clear()
            self.power_usage_cpu_values.clear()
            self.start_analyze = True
            self.timer.start()
            while self.start_analyze:
                powerusage = pynvml.nvmlDeviceGetPowerUsage(handle)
                self.power_usage_gpu_values.append(powerusage/1000)
 
                s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
                host = socket.gethostname()
                port = 8888
                s.connect((host, port))
                msg = s.recv(1024)
                s.close()
                self.power_usage_cpu_values.append(float(msg.decode('utf-8')))
 
                time.sleep(delay)
        self.thread = threading.Thread(target=start, daemon=True)
        self.thread.start()
 
    def analyze_end(self, mean=True):
        self.start_analyze = False
        while self.thread and self.thread.isAlive():
            time.sleep(0.01)
        time_used = self.timer.elapsed()
        self.thread = None
        power_usage_gpu = statistics.mean(self.power_usage_gpu_values) if mean else self.power_usage_gpu_values
        power_usage_cpu = statistics.mean(self.power_usage_cpu_values) if mean else self.power_usage_cpu_values
        return time_used, power_usage_gpu, power_usage_cpu
 
 
power_usage = PowerUsage()
def power_usage_api(func, note=''):
    @wraps(func)
    def wrapper(*args, **kwargs):
        power_usage.analyze_start()
        result = func(*args, **kwargs)
        print(f'{note}{power_usage.analyze_end()}')
        return result
    return wrapper
 
def power_usage_api2(note=''):
    def decorator(func):
        @wraps(func)
        def wrapper(*args, **kwargs):
            power_usage.analyze_start()
            result = func(*args, **kwargs)
            print(f'{note}{power_usage.analyze_end()}')
            return result
        return wrapper
    return decorator
```

# sudo 的困扰与解决

        上面提到，由于必须要 sudo 方式，但 sudo python 就换了**运行脚本的环境**了呀，这个比较棘手。后来想了个方法，曲线救国一下。**通过 sudo 运行一个脚本，并开启 socket 监听；而我们自己真正的脚本，在需要获取 CPU 功耗时候，连接一下 socket 就行。**

        为什么这里使用 socket 而不是 http 呢？因为 socket **更高效**一点！

我们写一个 “power_listener.py” 来监听：

```
power_usage = PowerUsage()
power_usage.analyze_start()
# ----------------------
# xxx 某一段待分析的代码
# 这里以sleep表示运行时长
time.sleep(2)
# ----------------------
time_used, power_usage_gpu, power_usage_cpu = power_usage.analyze_end()
print(f'time_used: {time_used}')
print(f'power_usage_gpu: {power_usage_gpu}')
print(f'power_usage_cpu: {power_usage_cpu}')
```

        因此，在需要获取 CPU 功耗时候，只需要：

```
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
host = socket.gethostname()
port = 8888
s.connect((host, port))
msg = s.recv(1024)
s.close()
power_usage_cpu = float(msg.decode('utf-8'))
```

![](https://img-blog.csdnimg.cn/c8aa098c64be49d2b3309fcf124f6037.png)

# 完整功耗分析示例代码

        提供一个我自己编写和使用的功耗分析代码，仅供参考。（注意上面的 power_listener.py 需要运行着）

```
import cv2
import socket
import sys
import threading
import json
import statistics
from psutil import _common as common
import pynvml
pynvml.nvmlInit()
 
class Timer: 
    def __init__(self, name = '', is_verbose = False):
        self._name = name 
        self._is_verbose = is_verbose
        self._is_paused = False 
        self._start_time = None 
        self._accumulated = 0 
        self._elapsed = 0         
        self.start()
 
    def start(self):
        self._accumulated = 0         
        self._start_time = cv2.getTickCount()
 
    def pause(self): 
        now_time = cv2.getTickCount()
        self._accumulated += (now_time - self._start_time)/cv2.getTickFrequency() 
        self._is_paused = True   
 
    def resume(self): 
        if self._is_paused: # considered only if paused 
            self._start_time = cv2.getTickCount()
            self._is_paused = False                      
 
    def elapsed(self):
        if self._is_paused:
            self._elapsed = self._accumulated
        else:
            now = cv2.getTickCount()
            self._elapsed = self._accumulated + (now - self._start_time)/cv2.getTickFrequency()        
        if self._is_verbose is True:      
            name =  self._name
            if self._is_paused:
                name += ' [paused]'
            message = 'Timer::' + name + ' - elapsed: ' + str(self._elapsed) 
            timer_print(message)
        return self._elapsed   
 
class PowerUsage:
    '''
    demo:
        power_usage = PowerUsage()
        power_usage.analyze_start()
        time.sleep(2)
        time_used, power_usage_gpu, power_usage_cpu = power_usage.analyze_end()
        print(time_used)
        print(power_usage_gpu)
        print(power_usage_cpu)
    '''
    def __init__(self):
        self.start_analyze = False
        self.power_usage_gpu_values = list()
        self.power_usage_cpu_values = list()
        self.thread = None
        self.timer = Timer(name='GpuPowerUsage', is_verbose=False)
 
    def analyze_start(self, gpu_id=0, delay=0.1):
        handle = pynvml.nvmlDeviceGetHandleByIndex(gpu_id)
        def start():
            self.power_usage_gpu_values.clear()
            self.power_usage_cpu_values.clear()
            self.start_analyze = True
            self.timer.start()
            while self.start_analyze:
                powerusage = pynvml.nvmlDeviceGetPowerUsage(handle)
                self.power_usage_gpu_values.append(powerusage/1000)
 
                s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
                host = socket.gethostname()
                port = 8888
                s.connect((host, port))
                msg = s.recv(1024)
                s.close()
                self.power_usage_cpu_values.append(float(msg.decode('utf-8')))
 
                time.sleep(delay)
        self.thread = threading.Thread(target=start, daemon=True)
        self.thread.start()
 
    def analyze_end(self, mean=True):
        self.start_analyze = False
        while self.thread and self.thread.isAlive():
            time.sleep(0.01)
        time_used = self.timer.elapsed()
        self.thread = None
        power_usage_gpu = statistics.mean(self.power_usage_gpu_values) if mean else self.power_usage_gpu_values
        power_usage_cpu = statistics.mean(self.power_usage_cpu_values) if mean else self.power_usage_cpu_values
        return time_used, power_usage_gpu, power_usage_cpu
 
 
power_usage = PowerUsage()
def power_usage_api(func, note=''):
    @wraps(func)
    def wrapper(*args, **kwargs):
        power_usage.analyze_start()
        result = func(*args, **kwargs)
        print(f'{note}{power_usage.analyze_end()}')
        return result
    return wrapper
 
def power_usage_api2(note=''):
    def decorator(func):
        @wraps(func)
        def wrapper(*args, **kwargs):
            power_usage.analyze_start()
            result = func(*args, **kwargs)
            print(f'{note}{power_usage.analyze_end()}')
            return result
        return wrapper
    return decorator
```

        用法示例：

```
power_usage = PowerUsage()
power_usage.analyze_start()
# ----------------------
# xxx 某一段待分析的代码
# 这里以sleep表示运行时长
time.sleep(2)
# ----------------------
time_used, power_usage_gpu, power_usage_cpu = power_usage.analyze_end()
print(f'time_used: {time_used}')
print(f'power_usage_gpu: {power_usage_gpu}')
print(f'power_usage_cpu: {power_usage_cpu}')
```

![](https://img-blog.csdnimg.cn/6b03cb5c6dec4894a8b38d1ff3597f6e.png)