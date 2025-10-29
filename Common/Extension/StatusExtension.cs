using Common.Enum;

namespace Common.Extension
{

    public static class StatusExtension
    {
        public static bool IsFinalize(this StatusEnum status) => status == StatusEnum.SUCCESS;
    }
}
