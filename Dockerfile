# Use gcc:11 base image for compiling the C code
FROM gcc:11 AS build

# Set the working directory
WORKDIR /build

# Copy the C code into the build container
COPY tcp_server_rst.c .

# Compile the C code
RUN gcc tcp_server_rst.c -o tcp_server_rst

# Use Debian bullseye base image for the final runtime environment
FROM debian:bullseye-slim

# Set the working directory
WORKDIR /app

# Copy the compiled binary from the build container
COPY --from=build /build/tcp_server_rst /app/tcp_server_rst

# Expose the server port
EXPOSE 5000

# Start the server
CMD ["/app/tcp_server_rst"]
