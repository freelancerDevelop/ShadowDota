﻿using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using JetBrains.Annotations;

namespace NetMQ.zmq.Utils
{
    /// <summary>
    /// This class exists only to provide a wrapper for the Socket.Select method,
    /// such that the call may be handled slightly differently on .NET 3.5 as opposed to later versions.
    /// </summary>
    internal static class SocketUtility
    {
        /// <summary>
        /// Determine the status of one or more sockets.
        /// After returning, the lists will be filled with only those sockets that satisfy the conditions.
        /// </summary>
        /// <param name="checkRead">a list of Sockets to check for readability</param>
        /// <param name="checkWrite">a list of Sockets to check for writability</param>
        /// <param name="checkError">a list of Sockets to check for errors</param>
        /// <param name="microSeconds">a timeout value, in microseconds. A value of -1 indicates an infinite timeout.</param>
        /// <remarks>
        /// If you are in a listening state, readability means that a call to Accept will succeed without blocking. 
        /// If you have already accepted the connection, readability means that data is available for reading. In these cases,
        /// all receive operations will succeed without blocking. Readability can also indicate whether the remote Socket
        /// has shut down the connection - in which case a call to Receive will return immediately, with zero bytes returned.
        /// 
        /// Select returns when at least one of the sockets of interest (ie any of the sockets in the checkRead, checkWrite, or checkError
        /// lists) meets its specified criteria, or the microSeconds parameter is exceeded - whichever comes first.
        /// Setting microSeconds to -1 specifies an infinite timeout.
        /// 
        /// If you make a non-blocking call to Connect, writability means that you have connected successfully. If you already
        /// have a connection established, writability means that all send operations will succeed without blocking.
        /// If you have made a non-blocking call to Connect, the checkError parameter identifies sockets that have not connected successfully.
        /// 
        /// See this reference for further details of the operation of the Socket.Select method:
        /// https://msdn.microsoft.com/en-us/library/system.net.sockets.socket.select(v=vs.110).aspx
        /// 
        /// This may possibly throw an ArgumentNullException, if all three lists are null or empty,
        /// and a SocketException if an error occurred when attempting to access a socket.
        /// 
        /// Use the Poll method if you only want to determine the status of a single Socket.
        /// 
        /// This method cannot detect certain kinds of connection problems,
        /// such as a broken network cable, or that the remote host was shut down ungracefully.
        /// You must attempt to send or receive data to detect these kinds of errors.
        /// </remarks>
        public static void Select([CanBeNull] IList checkRead, [CanBeNull] IList checkWrite, [CanBeNull] IList checkError, int microSeconds)
        {

			List<Socket> m_checkRead = new List<Socket>();
			foreach(Socket s in checkRead) {
				m_checkRead.Add(s);
			}


            // .NET 3.5 has a bug, such that -1 is not blocking the select call - therefore we use here instead the maximum integer value.
            if (microSeconds == -1)
                microSeconds = int.MaxValue;

            
            Socket.Select(checkRead, checkWrite, checkError, microSeconds);



			if (m_checkRead.Count > 0 && checkRead.Count == 0 && checkWrite.Count == 0 && checkError.Count == 0)
			{
				// Ok, OS X and iOS workaround.
				// Because of platform specific behaviour regarding Socket.Select
				// we might have relased since a connection was established but the socket is not in the read list.
				// Detect this by checking the readList using poll with 0 timeout.

				foreach (Socket s in m_checkRead) {
					if (s.Poll(0, SelectMode.SelectRead)) {
						checkRead.Add(s);
					}
				}
			}

        }
    }
}
