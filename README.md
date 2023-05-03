# TestTcpServer

This is a TCP server to help with troubleshooting `kubectl port-forward` handling of "connection reset by peer".

It instantly sends an RST packet upon accepting a connection.

## Usage

Create a pod running the test server:
```shell
kubectl run test --image=brianpursley/test-tcp-server:latest --image-pull-policy=Always
```

Start port forwarding:
```shell
kubectl port-forward test 5000
```

Use `nc` to connect to the server:
```shell
nc localhost 5000
```

kubectl port-forward cannot handle RST packets, causing the port forwarding session to terminate, even though the server is still running and able to processing additional connections.
```shell
Forwarding from 127.0.0.1:5000 -> 5000
Forwarding from [::1]:5000 -> 5000
Handling connection for 5000
E0501 22:29:54.202584   34030 portforward.go:406] an error occurred forwarding 5000 -> 5000: error forwarding port 5000 to pod f5b845de80f024124808e104b5ecbcf16310fb9f773cae7d9e2ce2f4e58d9032, uid : failed to execute portforward in network namespace "/var/run/netns/cni-e08e1861-e560-8739-b2ae-1eb3fca44f34": read tcp4 127.0.0.1:46224->127.0.0.1:5000: read: connection reset by peer
E0501 22:29:54.203535   34030 portforward.go:234] lost connection to pod
```

