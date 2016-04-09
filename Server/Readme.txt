IP TO PORT

iptables -t nat -A PREROUTING -p tcp -d 185.103.198.38  --jump DNAT --to-destination 185.103.198.229:81
iptables -t nat -A PREROUTING -p tcp -d 185.103.198.34  --jump DNAT --to-destination 185.103.198.229:8080
