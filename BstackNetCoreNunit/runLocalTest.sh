
python -m SimpleHTTPServer 8888 &
pid=$!
dotnet test --filter "LocalTest"
kill $pid