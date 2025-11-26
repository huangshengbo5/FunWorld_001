package util

import (
	"bytes"
	"encoding/json"
	"fmt"
	"github.com/sdvdxl/dinghook"
	"io/ioutil"
	"net/http"
	"time"
)

func DingDingAlert(content string) {
	ding := dinghook.Ding{AccessToken: "YOUR_DINGTALK_ACCESS_TOKEN"}
	msg := dinghook.Message{Content: "laoyang: " + content, AtAll: true}
	Result := ding.Send(msg)
	if !Result.Success {

	}
}

func FeishuAlert(content string) {
	sendWebhook("https://open.feishu.cn/open-apis/bot/v2/hook/3f9963a7-5e7f-4556-976c-98935178dc7c", content)
}

// WebhookRequest represents the payload sent to the Feishu webhook.
type WebhookRequest struct {
	MsgType string `json:"msg_type"`
	Content struct {
		Text string `json:"text"`
	} `json:"content"`
}

// WebhookResponse represents the response from the Feishu webhook.
type WebhookResponse struct {
	Code int    `json:"code"`
	Msg  string `json:"msg"`
}

// sendWebhook sends a message to the Feishu webhook and returns the response or an error.
func sendWebhook(webhookURL string, message string) error {
	// Create the request payload
	payload := WebhookRequest{
		MsgType: "text",
	}
	payload.Content.Text = message

	// Serialize the payload to JSON
	payloadBytes, err := json.Marshal(payload)
	if err != nil {
		return fmt.Errorf("failed to serialize payload: %v", err)
	}

	// Create an HTTP POST request
	req, err := http.NewRequest(http.MethodPost, webhookURL, bytes.NewBuffer(payloadBytes))
	if err != nil {
		return fmt.Errorf("failed to create HTTP request: %v", err)
	}
	req.Header.Set("Content-Type", "application/json")

	// Perform the HTTP request with a timeout
	client := &http.Client{Timeout: 10 * time.Second}
	resp, err := client.Do(req)
	if err != nil {
		return fmt.Errorf("failed to send HTTP request: %v", err)
	}
	defer resp.Body.Close()

	// Read and parse the response body
	respBody, err := ioutil.ReadAll(resp.Body)
	if err != nil {
		return fmt.Errorf("failed to read response body: %v", err)
	}

	// Check the HTTP status code
	if resp.StatusCode != http.StatusOK {
		return fmt.Errorf("non-200 HTTP status code: %d, body: %s", resp.StatusCode, respBody)
	}

	// Parse the JSON response
	var webhookResp WebhookResponse
	if err := json.Unmarshal(respBody, &webhookResp); err != nil {
		return fmt.Errorf("failed to parse JSON response: %v", err)
	}

	// Check the Feishu API response code
	if webhookResp.Code != 0 {
		return fmt.Errorf("Feishu API error: code=%d, msg=%s", webhookResp.Code, webhookResp.Msg)
	}

	fmt.Println("Message sent successfully!")
	return nil
}
