package main

import (
	"context"
	"fmt"
	"log"
	"strconv"
	"time"

	eventhub "github.com/Azure/azure-event-hubs-go"
)

const (
	eventHubMessageThreshold       = 10
	eventHubNamespace              = "<eventhub-namespace>"
	eventHubKey                    = "<eventhub-key>"
	eventHubName                   = "<eventhub-name>"
	eventHubConnectionStringFormat = "Endpoint=sb://%s.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=%s;EntityPath=%s"
)

func main() {
	eventHubConnectionString := fmt.Sprintf(eventHubConnectionStringFormat, eventHubNamespace, eventHubName)

	hub, err := eventhub.NewHubFromConnectionString(eventHubConnectionString)
	if err != nil {
		log.Fatalf("failed to create hub client\n\n%s", err)
	}

	ctx, cancel := context.WithTimeout(context.Background(), 10*time.Second)
	defer hub.Close(ctx)
	defer cancel()
	if err != nil {
		log.Fatalf("failed to get hub %s\n", err)
	}

	// Send 50 messages to event hub
	ctx = context.Background()

	for i := 0; i < 50; i++ {
		text := strconv.Itoa(i)
		err := hub.Send(ctx, eventhub.NewEventFromString(text))
		if err != nil {
			fmt.Printf("Error sending msg: %s\n", err)
		} else {
			fmt.Printf("Sent message\n")
		}
	}
}
