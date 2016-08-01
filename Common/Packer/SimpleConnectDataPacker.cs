using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Common.Packer.Vo;
using Common.Constant;

namespace Common.Packer
{
    public class SimpleConnectDataPacker
    {
        public static PackVo unpackingData(Socket socket)
        {
            PackVo vo = new PackVo();
            readAndCheckUnit(socket, PackerConstant.BOU);
            int iLength = readLength(socket);
            byte[] bData = readBytesByLength(socket, iLength);
            if (readAndCheckUnit(socket, PackerConstant.EOU))
            {
                byte[] bOP_Code = new byte[PackerConstant.OP_LENGTH];
                byte[] bVoData = new byte[bData.Length - PackerConstant.OP_LENGTH];
                Array.Copy(bData, 0, bOP_Code, 0, PackerConstant.OP_LENGTH);
                Array.Copy(bData, PackerConstant.OP_LENGTH, bVoData, 0, bVoData.Length);
                vo.OP_Code = bOP_Code[0];
                vo.Data = bVoData;
            }
            else
            {
                return unpackingData(socket);
            }
            return vo;
        }

        public static byte[] packingData(PackVo vo)
        {
            int iTotalLength = PackerConstant.BOU_EOU_LENGTH + PackerConstant.LENGTH_LENGTH + PackerConstant.OP_LENGTH + PackerConstant.BOU_EOU_LENGTH;
            iTotalLength += vo.Data.Length;
            byte[] bData = new byte[iTotalLength];
            int iOffset = 0;
            bData = writeBOU(bData, iOffset);
            iOffset += PackerConstant.BOU_EOU_LENGTH;
            bData = writeLength(bData, vo, iOffset);
            iOffset += PackerConstant.LENGTH_LENGTH;
            bData = writePackVo(bData, vo, iOffset);
            iOffset += PackerConstant.OP_LENGTH + vo.Data.Length;
            bData = writeEOU(bData, iOffset);
            return bData;
        }

        public static byte[] packingData(byte[] bData)
        {
            PackVo vo = new Vo.PackVo();
            vo.OP_Code = OP_Code.DATA;
            vo.Data = bData;
            return packingData(vo);
        }

        public static byte[] packingHeartbeat()
        {
            PackVo vo = new Vo.PackVo();
            vo.OP_Code = OP_Code.HEARTBEAT;
            vo.Data = new byte[0];
            return packingData(vo);
        }

        private static bool readAndCheckUnit(Socket socket, byte bUnit)
        {
            int iReadLength = 0;
            while(iReadLength != PackerConstant.BOU_EOU_LENGTH)
            {
                byte[] b = readBytesByLength(socket, 1);
                if (b != null && b[0] == bUnit)
                {
                    iReadLength++;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        private static int readLength(Socket socket)
        {
            byte[] b = readBytesByLength(socket, PackerConstant.LENGTH_LENGTH);
            int iLength = 0;
		    for(int i = 0; i<b.Length ; i++) {
			    iLength *= PackerConstant.POSITIVE_BYTE_SIZE;
			    iLength += Convert.ToInt32(b[i]);
		    }
		    return iLength;
	    }

        private static byte[] readBytesByLength(Socket socket, int iLength)
        {
            byte[] bData = new byte[iLength];
            int iReadLength = 0;
            int iReadTotalLength = 0;
		
		    while(iReadTotalLength<iLength)
            {
                try
                {
                    iReadLength = socket.Receive(bData, iReadLength, iLength - iReadLength, SocketFlags.None);
                    iReadTotalLength += iReadLength;
                } catch
                {
                    throw;
                }
		    }
		    return bData;
	    }

        private static byte[] writeBOU(byte[] bData, int iOffset)
        {
            for (int i = 0; i < PackerConstant.BOU_EOU_LENGTH; i++)
            {
                bData[iOffset + i] = PackerConstant.BOU;
            }
            return bData;
        }

        private static byte[] writeEOU(byte[] bData, int iOffset)
        {
            for(int i = 0; i < PackerConstant.BOU_EOU_LENGTH ; i++)
            {
                bData[iOffset + i] = PackerConstant.EOU;
            }
            return bData;
        }

        private static byte[] writeLength(byte[] bData, PackVo vo, int iOffset)
        {
            int iLength = PackerConstant.OP_LENGTH + vo.Data.Length;
            byte[] bLength = new byte[PackerConstant.LENGTH_LENGTH];
            bLength[0] = BitConverter.GetBytes(iLength / PackerConstant.POSITIVE_BYTE_SIZE)[0];
            bLength[1] = BitConverter.GetBytes(iLength % PackerConstant.POSITIVE_BYTE_SIZE)[0];
            Array.Copy(bLength, 0, bData, iOffset, PackerConstant.LENGTH_LENGTH);
            return bData;
        }

        private static byte[] writePackVo(byte[] bData, PackVo vo, int iOffset)
        {
            bData[iOffset] = vo.OP_Code;
            int iDataLength = vo.Data.Length;
            Array.Copy(vo.Data, 0, bData, iOffset + PackerConstant.OP_LENGTH, iDataLength);
            return bData;
        }
    }
}
