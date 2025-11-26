package main

import "fmt"

type Player struct {
	ID int
}

func main() {
	playerList := []Player{}
	for i := 0; i < 100; i++ {
		playerList = append(playerList, Player{i})
	}

	for _, player := range playerList {
		go func() {
			fmt.Println(player.ID)
		}()
	}
}
