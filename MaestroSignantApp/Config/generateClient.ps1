nswag run pricing-tool-config.nswag version /runtime:Net60

(Get-Content ../../MaestroSignantUI/src/apiclient/apiclient_generated.ts).replace('const _responseText = response.data;', 'const _responseText: any = response.data;') | Set-Content ../../MaestroSignantUI/src/apiclient/apiclient_generated.ts