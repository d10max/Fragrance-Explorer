interface PerfumeBottleProps {
    capColor?: string;
    liquidColor?: string;
    liquidHeight?: number;
    width?: number | string;
    height?: number | string;
    className?: string;
}

export const PerfumeBottle = ({
    capColor = '#18181b',
    liquidColor = '#ebd788',
    liquidHeight = 44,
    width = 86,
    height = 126,
    className,
}: PerfumeBottleProps) => {
    return (
        <svg
            width={width}
            height={height}
            viewBox="0 0 86 126"
            fill="none"
            xmlns="http://www.w3.org/2000/svg"
            className={className}
        >
            {/* cover */}
            <rect x="31" y="8" width="24" height="20" rx="1.5" fill={capColor} />

            {/* cover part */}
            <rect x="39" y="28" width="8" height="6" fill="#c59b27" />

            {/* body of bottle */}
            <rect
                x="15"
                y="34"
                width="56"
                height="82"
                rx="4"
                fill="#ffffff"
                stroke="#e2ddd5"
                strokeWidth="1.5"
            />

            {/* level of liquid */}
            {liquidColor && (
                <rect
                    x="17"
                    y={114 - liquidHeight}
                    width="52"
                    height={liquidHeight}
                    rx="2"
                    fill={liquidColor}
                    opacity="0.88"
                />
            )}

            {/* label */}
            <rect
                x="21"
                y="46"
                width="44"
                height="48"
                rx="2"
                fill="#ffffff"
                stroke="#ede8e1"
                strokeWidth="1"
            />
        </svg>
    );
};
