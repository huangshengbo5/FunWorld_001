package util

import (
	"bytes"
	"crypto/tls"
	"io/ioutil"
	"net/http"
	"time"
)

const (
	httpTimeout = 60 * time.Second
)

//
//type HttpRequest struct {
//	url     string
//	method  Method
//	header  map[Header]string
//	cookies []*http.Cookie
//	body    []byte
//}
//
//type Method string
//
//const (
//	MethodPost Method = "post"
//	MethodGet  Method = "get"
//)
//
//type Header string
//
//const (
//	HeaderContentType   Header = "Content-Type"
//	HeaderConnection    Header = "Connection"
//	HeaderRefer         Header = "Referer"
//	HeaderAuthorization Header = "Authorization"
//	HeaderUserAgent     Header = "User-Agent"
//)
//
//func NewHttpRequest(url string, method Method, header map[Header]string) *HttpRequest {
//	if header == nil {
//		header = map[Header]string{
//			HeaderContentType: "application/x-www-form-urlencoded",
//		}
//	}
//	return &HttpRequest{
//		url:    url,
//		method: method,
//		header: header,
//	}
//}
//
//func (req *HttpRequest) SetQueryString(data interface{}) *HttpRequest {
//	body, err := urlquery.Marshal(data)
//
//	if err != nil {
//		logbus.Error(zap.String("http.SetQueryString", err.Error()))
//	}
//
//	req.body = body
//	return req
//}
//
//func (req *HttpRequest) SetHeaders(headers map[Header]string) *HttpRequest {
//	for k, v := range headers {
//		req.header[k] = v
//	}
//
//	return req
//}
//
//func (req *HttpRequest) SetHeader(k Header, v string) *HttpRequest {
//	req.header[k] = v
//	return req
//}
//
//func (req *HttpRequest) SetCookie(name, value string) *HttpRequest {
//	req.cookies = append(req.cookies, &http.Cookie{
//		Name:  name,
//		Value: value,
//	})
//	return req
//}
//
//type HttpClient struct {
//	client   *http.Client
//	response *http.Response
//}
//
//func NewHttpsClient(timeout time.Duration) *HttpClient {
//	return &HttpClient{
//		client: &http.Client{
//			Transport: &http.Transport{
//				TLSClientConfig: &tls.Config{
//					InsecureSkipVerify: false,
//				}},
//			Timeout: timeout,
//		},
//	}
//}
//
//func NewHttpClient(timeout time.Duration) *HttpClient {
//	return &HttpClient{
//		client: &http.Client{
//			Timeout: timeout,
//		},
//	}
//}
//
//func (client *HttpClient) SetTimeout(timeout time.Duration) {
//	client.client.Timeout = timeout
//}
//
//func (client *HttpClient) Do(request *HttpRequest) (body []byte, err error) {
//	var req *http.Request
//	req, err = http.NewRequest(string(request.method), request.url, bytes.NewBuffer(request.body))
//	if err != nil {
//		return
//	}
//
//	// 设置header和cookie
//	if request != nil {
//		for key, value := range request.header {
//			req.Header.Set(string(key), value)
//		}
//
//		for _, cookie := range request.cookies {
//			req.AddCookie(cookie)
//		}
//	}
//
//	client.response, err = client.client.Do(req)
//	if err != nil {
//		return nil, err
//	}
//
//	if client.response != nil {
//		defer func() {
//			_, _ = io.Copy(ioutil.Discard, client.response.Body)
//			_ = client.response.Body.Close()
//		}()
//	}
//
//	return ioutil.ReadAll(client.response.Body)
//}
//
//func (client *HttpClient) GetStatusCode() int {
//	if client.response != nil {
//		return client.response.StatusCode
//	}
//
//	return -1
//}

const (
	ContentTypeForm = "application/x-www-form-urlencoded"
	ContentTypeJson = "application/json"
)

var (
	defaultClient = &http.Client{
		Timeout: httpTimeout,
	}

	httpsClient = &http.Client{
		Transport: &http.Transport{
			TLSClientConfig: &tls.Config{
				InsecureSkipVerify: false,
			}},
		Timeout: httpTimeout,
	}
)

func HttpsPost(url string, postData []byte, header map[string]string, contentType string) (respData []byte, err error) {
	var req *http.Request
	req, err = http.NewRequest("POST", url, bytes.NewReader(postData))
	if err != nil {
		return nil, err
	}

	for k, v := range header {
		req.Header.Set(k, v)
	}

	req.Header.Set("Content-Type", contentType)

	var resp *http.Response
	resp, err = httpsClient.Do(req)
	if err != nil {
		return
	}
	defer resp.Body.Close()

	respData, err = ioutil.ReadAll(resp.Body)
	if err != nil {
		return
	}

	return
}

func HttpsGet(url string, header map[string]string, contentType string) (respData []byte, err error) {
	var req *http.Request
	req, err = http.NewRequest("GET", url, nil)
	if err != nil {
		return nil, err
	}

	for k, v := range header {
		req.Header.Set(k, v)
	}
	req.Header.Set("Content-Type", contentType)

	return httpDo(httpsClient, req)
}

func HttpGet(url string, header map[string]string, contentType string) (respData []byte, err error) {
	var req *http.Request
	req, err = http.NewRequest("GET", url, nil)
	if err != nil {
		return nil, err
	}

	for k, v := range header {
		req.Header.Set(k, v)
	}
	req.Header.Set("Content-Type", contentType)

	return httpDo(defaultClient, req)
}

func httpDo(client *http.Client, req *http.Request) (respData []byte, err error) {
	var resp *http.Response
	resp, err = client.Do(req)
	if err != nil {
		return
	}
	defer resp.Body.Close()

	return ioutil.ReadAll(resp.Body)
}
